using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Workflow.Models
{
    public class NodeConfig
    {
        public string NodeType { get; set; }
        public string NodeDescription { get; set; }
        public EnumNodeStartMode StartMode { get; set; }
        public Dictionary<string, NodeConfig> NextNodes { get; set; }
    }

    public class FlowConfig
    {
        public string FlowName { get; set; }
        public string EntranceNodeNodeType { get; set; }
        public List<NodeConfig> AvailableNodes { get; set; }
    }
}
