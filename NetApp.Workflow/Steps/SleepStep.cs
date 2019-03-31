using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace NetApp.Workflow.Steps
{
    public class SleepStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (context.PersistenceData == null)
                return ExecutionResult.Sleep(TimeSpan.FromHours(12), new object());
            else
                return ExecutionResult.Next();
        }
    }
}
