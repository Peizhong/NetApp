using NetApp.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace NetApp.Workflow
{
    public class CreateOrderNode : Node
    {
        public override string NodeName => "CreateOrderNode";
        public override string NodeDescription { get; protected set; }
        public override string NodeType { get; protected set; }

        private const string _createCmd = "CreateOrder";
        
        private readonly ILogger<CreateOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;
        
        public CreateOrderNode(ILogger<CreateOrderNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            ExcuteCondition = EnumNodeExcuteCondition.Any;
        }

        public override bool IsCanExcute(string command)
        {
            //当前节点接受的命令类型
            if (command == _createCmd)
            {
                _logger.LogDebug($"command {command} accepted");
                return true;
            }
            return false;
        }

        public override async Task DoWork()
        {
            try
            {
                Message message = JsonConvert.DeserializeObject<Message>(_data);
                using (var scope = _serviceProvider.CreateScope())
                {
                    using(var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                    {
                        await context.Messages.AddAsync(message);
                        await context.SaveChangesAsync();
                    }
                }
                NodeStatus = EnumNodeStatus.Complete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                NodeStatus = EnumNodeStatus.Error;
            }
        }
    }


    public class ApproveNode : Node
    {
        public override string NodeName => "ApproveNode";
        public override string NodeDescription { get; protected set; }
        public override string NodeType { get; protected set; }

        private readonly ILogger<ApproveNode> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ApproveNode(ILogger<ApproveNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            ExcuteCondition = EnumNodeExcuteCondition.Any;
        }

        public override async Task DoWork()
        {
            try
            {
                var id = _data;
                if (!string.IsNullOrEmpty(id))
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        using (var context = scope.ServiceProvider.GetRequiredService<NetAppDbContext>())
                        {
                            var message = await context.Messages.FindAsync(id);
                            if (message != null)
                            {
                                message.Status = 2;
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

    public class RejectOrderNode : Node
    {
        public override string NodeName => "RejectOrderNode";
        public override string NodeDescription { get; protected set; }
        public override string NodeType { get; protected set; }

        private readonly ILogger<RejectOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;
        

        public RejectOrderNode(ILogger<RejectOrderNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            ExcuteCondition = EnumNodeExcuteCondition.Any;
        }

        public override async Task DoWork()
        {
            try
            {
                var id = _data;
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
        public override string NodeName => "RejectOrderNode";
        public override string NodeDescription { get; protected set; }
        public override string NodeType { get; protected set; }

        private readonly ILogger<CompleteOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;


        public CompleteOrderNode(ILogger<CompleteOrderNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            ExcuteCondition = EnumNodeExcuteCondition.Any;
        }

        public override async Task DoWork()
        {
            try
            {
                var id = _data;
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
        public override string NodeName => "RejectOrderNode";
        public override string NodeDescription { get; protected set; }
        public override string NodeType { get; protected set; }

        private readonly ILogger<CancelOrderNode> _logger;
        private readonly IServiceProvider _serviceProvider;


        public CancelOrderNode(ILogger<CancelOrderNode> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            ExcuteCondition = EnumNodeExcuteCondition.Any;
        }

        public override async Task DoWork()
        {
            try
            {
                var id = _data;
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
