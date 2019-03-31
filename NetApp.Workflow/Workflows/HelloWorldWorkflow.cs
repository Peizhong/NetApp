using NetApp.Workflow.Steps;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace NetApp.Workflow.Workflows
{
    public class HelloWorldWorkflow : IWorkflow
    {
        public string Id => "HelloWorld";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<HelloWorld>()
                .Then<GoodbyeWorld>();
        }
    }
}
