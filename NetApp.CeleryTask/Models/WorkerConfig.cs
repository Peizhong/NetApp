using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class WorkerConfig
    {
        public string QueueName { get; set; } = "CTASK_QUEUE";

        /// <summary>
        /// 在此之前的事件不再执行
        /// </summary>
        public int OverdueMilliseconds { get; set; }
    }
}
