using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetApp.Workflow.Models
{
    public enum EnumNodeStatus
    {
        None = 0,
        Create = 0x1,
        Excuting = 0x10,
        Complete = 0x100,
        Abort = 0x1000,
        Error = 0x10000,
    }

    public enum EnumNodeStartMode
    {
        /// <summary>
        /// 直接往下执行
        /// </summary>
        Direct,
        /// <summary>
        /// 上级全部执行完毕
        /// </summary>
        All,
        /// <summary>
        /// 任意一个上级执行完毕
        /// </summary>
        Any,
    }

    public class Node
    {
        public Node()
        {
        }

        public string NodeId { get; private set; }

        public string FlowId { get; set; }

        public Flow Flow { get; set; }

        /// <summary>
        /// assemly
        /// </summary>
        public string NodeType { get; set; }
        
        /// <summary>
        /// 当前状态时间
        /// </summary>
        public DateTime StatusTime { get; set; }

        /// <summary>
        /// 用三个节点进入的条件
        /// </summary>
        public EnumNodeStartMode StartMode { get; set; }

        public EnumNodeStatus NodeStatus { get; set; }

        protected string _inputCommand;
        protected string _inputData;

        private Dictionary<string, string> inputDataDict;
        public Dictionary<string, string> InputDataDict
        {
            get
            {
                if (inputDataDict == null)
                {
                    try
                    {
                        inputDataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(_inputData);
                    }
                    catch { inputDataDict = new Dictionary<string, string>(); };
                }
                return inputDataDict;
            }
        }

        public string OutputCommand { get; set; }
        public string OutputData { get; set; }
        
        /// <summary>
        /// base function do nothing, then completed the node
        /// </summary>
        public virtual Task DoWork()
        {
            NodeStatus = EnumNodeStatus.Complete;
            return Task.CompletedTask;
        }

        public async Task TryExecute(string command, string data)
        {
            if (NodeStatus == EnumNodeStatus.Create)
            {
                NodeStatus = EnumNodeStatus.Excuting;
                StatusTime = DateTime.Now;
            }
            _inputCommand = command;
            _inputData = data;
            await DoWork();
        }
    }
}
