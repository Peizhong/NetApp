using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetApp.CeleryTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetApp.CeleryTask
{
    public class TaskBeater
    {
        private string TASK_QUEUE_NAME = "CTASK_QUEUE";

        private readonly ILogger<TaskBeater> _logger;
        private readonly IServiceScopeFactory _serviceScope;

        public int IntervalMilliseconds { get; set; } = 1000;

        public TaskBeater(ILogger<TaskBeater> logger, IServiceScopeFactory serviceScope)
        {
            _logger = logger;
            _serviceScope = serviceScope;
        }

        Task beaterTask = null;
        CancellationTokenSource cancelBeater = null;

        private void setDefaultParams(PeriodicTask task)
        {
            if (string.IsNullOrEmpty(task.Params))
            {
                var dict = new Dictionary<string, object>();
                switch (task.TaskName)
                {
                    case "HelloString":
                        dict.Add("world", "Hello world");
                        break;
                    case "IntAndString":
                        dict.Add("num", 1);
                        dict.Add("str", "It is string 哦");
                        break;
                    default:
                        break;
                }
                task.Params = JsonConvert.SerializeObject(dict);
            }
        }

        /// <summary>
        /// write to rabbit
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private ExcuteResult createTask(PeriodicTask task)
        {
#if DEBUG
            setDefaultParams(task);
#endif
            RabbitHelper.Instance.Pubilsh(task, TASK_QUEUE_NAME);
            return new ExcuteResult
            {
                Code = "0000"
            };
        }

        /// <summary>
        /// write demo task
        /// </summary>
        /// <param name="tasks"></param>
        public void SayHi(IEnumerable<CTask> tasks)
        {
            var pTasks = tasks.Select(t => new PeriodicTask
            {
                Id = Guid.NewGuid().ToString(),
                TaskName = t.TaskName,
                IsActive = true,
                StartTime = DateTime.Now,
                Params = "",
                IntervalSchedule = new IntervalSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    Every = 2,
                    Period = EnumPeriod.Seconds,
                }
            }).ToList();
            using (var scope = _serviceScope.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<CeleryDbContext>())
                {
                    context.PeriodicTasks.AddRange(pTasks);
                    context.SaveChanges();
                }
            }
        }

        private void setNextTime(PeriodicTask task)
        {
            var now = DateTime.Now;
            Func<IntervalSchedule, int> convertMilliseconds = (schedule) =>
            {
                int seconds = 1;
                switch (schedule.Period)
                {
                    case EnumPeriod.Seconds:
                        seconds = 1;
                        break;
                    case EnumPeriod.Minutes:
                        seconds = 60;
                        break;
                    case EnumPeriod.Hours:
                        seconds = 60 * 60;
                        break;
                    case EnumPeriod.Days:
                        seconds = 60 * 60 * 24;
                        break;
                    default:
                        break;
                }
                return seconds * schedule.Every;
            };
            if (task.IntervalSchedule != null)
            {
                task.NextTime = now.AddSeconds(convertMilliseconds(task.IntervalSchedule));
            }
            else if (task.CrontabSchedule != null)
            {
                if (task.CrontabSchedule.DayOfMonth == "*")
                {

                }
                if (task.CrontabSchedule.DayOfWeek == "*")
                {

                }
                if (task.CrontabSchedule.Hour == "*")
                {

                }
                if (task.CrontabSchedule.Minute == "*")
                {

                }
            }
        }

        public ExcuteResult Run(string taskQueueName)
        {
            _logger.LogInformation($"Beater Run in queue {taskQueueName}");
            TASK_QUEUE_NAME = taskQueueName;

            cancelBeater = new CancellationTokenSource();
            CancellationToken token = cancelBeater.Token;
            beaterTask = Task.Run(async () =>
            {
                using (var scope = _serviceScope.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<CeleryDbContext>())
                    {
                        while (!token.IsCancellationRequested)
                        {
                            var now = DateTime.Now;
                            var todos = context.PeriodicTasks.Where(t => t.IsActive).ToList();
                            foreach (var todo in todos)
                            {
                                if (todo.StartTime.HasValue && todo.StartTime > now)
                                {
                                    continue;
                                }
                                if (todo.NextTime.HasValue && todo.NextTime > now)
                                {
                                    continue;
                                }
                                if (todo.Expires.HasValue && todo.Expires < now)
                                {
                                    continue;
                                }
                                createTask(todo);
                                setNextTime(todo);
                            }
                            await Task.Delay(IntervalMilliseconds);
                        }
                    }
                }
            }, token);

            return new ExcuteResult
            {
                Code = "0000"
            };
        }

        public ExcuteResult Stop()
        {
            _logger.LogInformation("Beater Stop");
            cancelBeater?.Cancel();
            return new ExcuteResult
            {
                Code = "0000"
            };
        }
    }
}
