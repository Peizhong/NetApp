using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;
using NetApp.CeleryTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetApp.CeleryTask
{
    public class TaskBeater
    {
        private string TASK_QUEUE_NAME = "CTASK_QUEUE";

        private readonly ILogger<TaskBeater> _logger;
        private readonly IServiceScopeFactory _serviceScope;

        private int intervalMilliseconds = 1000;
        private int overdueMilliseconds = 0;

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
                IntervalSchedule = new CTaskIntervalSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    Every = 10,
                    Period = EnumPeriod.Seconds,
                }
            }).ToList();
            pTasks[0].IntervalSchedule = null;
            pTasks[0].CrontabSchedule = new CTaskCrontabSchedule
            {
                Minute = "0",
                Hour = "3",
                DayOfWeek = "*",
                DayOfMonth = "*",
                MonthOfYear = "*"
            };
            using (var scope = _serviceScope.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<CeleryDbContext>())
                {
                    context.PeriodicTasks.AddRange(pTasks);
                    context.SaveChanges();
                }
            }
        }

        private DateTime getNextInterval(PeriodicTask task)
        {
            Func<CTaskIntervalSchedule, int> convertMilliseconds = (schedule) =>
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
            var next = task.NextTime.Value.AddSeconds(convertMilliseconds(task.IntervalSchedule));
            return next;
        }

        private DateTime getNextCrontab(PeriodicTask task)
        {
            var crontabStr = string.Join(" ",
                new[]
                {
                    task.CrontabSchedule.Minute,
                    task.CrontabSchedule.Hour,
                    task.CrontabSchedule.DayOfWeek,
                    task.CrontabSchedule.DayOfMonth,
                    task.CrontabSchedule.MonthOfYear
                }
            );
            var s = CrontabSchedule.Parse(crontabStr);
            var next = s.GetNextOccurrence(task.NextTime.Value);
            return next;
        }

        private void setNextTime(PeriodicTask task)
        {
            if (!task.NextTime.HasValue)
            {
                task.NextTime = DateTime.Now;
            }
            if (task.IntervalSchedule != null)
            {
                task.NextTime = getNextInterval(task);
            }
            else if (task.CrontabSchedule != null)
            {
                task.NextTime = getNextCrontab(task);
            }
        }

        public ExcuteResult Run(string taskQueueName, int interval = 1000, int overdue = 0)
        {
            _logger.LogInformation($"Beater Run in queue {taskQueueName}");
            TASK_QUEUE_NAME = taskQueueName;
            intervalMilliseconds = interval;
            overdueMilliseconds = overdue;

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
                            var todos = await context.PeriodicTasks.Include(p => p.IntervalSchedule).Include(p => p.CrontabSchedule).Where(p => p.IsActive).ToListAsync();
                            bool changed = false;
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
                                if (todo.NextTime.HasValue && todo.NextTime < now)
                                {
                                    if (overdueMilliseconds > 0)
                                    {
                                        //原计划执行时间 超过了设置的超时时间，丢弃前面的任务
                                        if ((now - todo.NextTime.Value).TotalMilliseconds > overdueMilliseconds)
                                        {
                                            todo.NextTime = now;
                                        }
                                    }
                                }
                                changed = true;
                                createTask(todo);
                                setNextTime(todo);
                                todo.IsActive = todo.NextTime.HasValue;
                            }
                            if (changed)
                            {
                                context.SaveChanges();
                            }
                            await Task.Delay(intervalMilliseconds);
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
