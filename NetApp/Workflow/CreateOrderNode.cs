using NetApp.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Workflow
{
    public class CreateOrderNode : Node
    {
        public override string NodeTypeName => "CreateOrderNode";

        public override bool IsCanExcute(string command)
        {
            return base.IsCanExcute(command);
        }

        public override string DoWork()
        {
            return base.DoWork();
        }
    }
}
