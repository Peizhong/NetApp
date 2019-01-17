using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class BeaterConfig
    {
        public string QueueName { get; set; } = "CTASK_QUEUE";

        /// <summary>
        /// 在此之前的事件不再触发
        /// </summary>
        public int OverdueMilliseconds { get; set; }

        /// <summary>
        /// beater检测间隔
        /// </summary>
        public int IntervalMilliseconds { get; set; } = 1000;
    }
}
