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
        private const string _createCmd = "Create";
        private const string _editCmd = "Save";
        private const string _submitCmd = "Submit";

        public bool IsCanExcute(string command)
        {
            //当前节点接受的命令类型
            if (command == _createCmd || command == _editCmd)
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
                        var dbData = await context.Messages.FindAsync(message.Id);
                        if (dbData != null)
                        {
                            if (_inputCommand == _editCmd)
                            {
                                message.Status = MessageStatus.Edit;
                            }
                            else if (_inputCommand == _submitCmd)
                            {
                                message.Status = MessageStatus.Approved;
                                NodeStatus = EnumNodeStatus.Complete;
                            }
                            context.Entry(dbData).CurrentValues.SetValues(message);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                OutputCommand = "DoIt";
                OutputData = message.Id;
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
                                if (message.Value == "ok")
                                {
                                    message.Status = MessageStatus.Approved;
                                    await context.SaveChangesAsync();
                                    NodeStatus = EnumNodeStatus.Complete;
                                    OutputData = id;
                                }
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
                                message.Status = MessageStatus.Reject;
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

    public class CompleteOrderNode : Node
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
                                message.Status = MessageStatus.Complete;
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

    public class CancelOrderNode : Node
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
                                message.Status =  MessageStatus.Cancel;
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
}
