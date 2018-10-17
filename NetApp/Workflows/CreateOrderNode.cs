using NetApp.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace NetApp.Workflows
{
    public class EditOrderNode : Node
    {
        private const string _createCmd = "CreateOrder";
        private const string _editCmd = "EditOrder";

        public bool IsCanExcute(string command)
        {
            //当前节点接受的命令类型
            if (command == _createCmd || command== _editCmd)
            {
                return true;
            }
            return false;
        }

        public override async Task DoWork()
        {
            if (!IsCanExcute(_inputCommand))
                return;
            try
            {
                Message message = JsonConvert.DeserializeObject<Message>(_inputData);
                using (var scope = Flow.ServiceProvider.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                    {
                        if (_inputCommand == _createCmd)
                        {
                            await context.Messages.AddAsync(message);
                        }
                        else if (_inputCommand == _editCmd)
                        {
                            var dbData = await context.Messages.FindAsync(message.Id);
                            if (dbData != null)
                            {
                                context.Entry(dbData).CurrentValues.SetValues(message);
                            }
                        }
                        await context.SaveChangesAsync();
                    }
                }
                OutputCommand = "DoIt";
                OutputData = message.Id;
                NodeStatus = EnumNodeStatus.Complete;
                StatusTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                NodeStatus = EnumNodeStatus.Error;
            }
        }
    }

    public class ApproveNode : Node
    {
        public override async Task DoWork()
        {
            try
            {
                var id = _inputData;
                if (!string.IsNullOrEmpty(id))
                {
                    using (var scope = Flow.ServiceProvider.CreateScope())
                    {
                        using (var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                        {
                            var message = await context.Messages.FindAsync(id);
                            if (message != null)
                            {
                                message.Status += 1;
                                await context.SaveChangesAsync();
                                NodeStatus = EnumNodeStatus.Complete;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NodeStatus = EnumNodeStatus.Error;
            }
        }
    }

    public class RejectOrderNode : Node
    {
        private readonly ILogger<RejectOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;
        
        public override async Task DoWork()
        {
            try
            {
                var id = _inputData;
                if (!string.IsNullOrEmpty(id))
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        using (var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                        {
                            var message = await context.Messages.FindAsync(id);
                            if (message != null)
                            {
                                message.Status = 3;
                                await context.SaveChangesAsync();
                                NodeStatus = EnumNodeStatus.Complete;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                NodeStatus = EnumNodeStatus.Error;
            }
        }
    }

    public class CompleteOrderNode : Node
    {
        private readonly ILogger<CompleteOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CompleteOrderNode(ILogger<CompleteOrderNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            StartMode = EnumNodeStartMode.Any;
        }

        public override async Task DoWork()
        {
            try
            {
                var id = _inputData;
                if (!string.IsNullOrEmpty(id))
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        using (var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                        {
                            var message = await context.Messages.FindAsync(id);
                            if (message != null)
                            {
                                message.Status = 9;
                                await context.SaveChangesAsync();
                                NodeStatus = EnumNodeStatus.Complete;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                NodeStatus = EnumNodeStatus.Error;
            }
        }
    }

    public class CancelOrderNode : Node
    {
        private readonly ILogger<CancelOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CancelOrderNode(ILogger<CancelOrderNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            StartMode = EnumNodeStartMode.Any;
        }

        public override async Task DoWork()
        {
            try
            {
                var id = _inputData;
                if (!string.IsNullOrEmpty(id))
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        using (var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                        {
                            var message = await context.Messages.FindAsync(id);
                            if (message != null)
                            {
                                message.Status = 4;
                                await context.SaveChangesAsync();
                                NodeStatus = EnumNodeStatus.Complete;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                NodeStatus = EnumNodeStatus.Error;
            }
        }
    }
}
