using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class PeriodicTask
    {
        [Key]
        public string Id { get; set; }

        public string TaskName { get; set; }

        public string Params { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartTime { get; set; }
        
        public DateTime? NextTime { get; set; }

        public DateTime? Expires { get; set; }

        public CTaskCrontabSchedule CrontabSchedule { get; set; }

        public CTaskIntervalSchedule IntervalSchedule { get; set; }
    }

    public class CTaskCrontabSchedule 
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// * every 
        /// </summary>
        public string Minute { get; set; }

        public string Hour { get; set; }

        public string DayOfWeek { get; set; }

        public string DayOfMonth { get; set; }

        public string MonthOfYear { get; set; }
    }

    public enum EnumPeriod
    {
        Days = 1,
        Hours = 2,
        Minutes = 3,
        Seconds = 4,
    }

    public class CTaskIntervalSchedule
    {
        [Key]
        public string Id { get; set; }

        public int Every { get; set; }
        
        public EnumPeriod Period { get; set; }
    }
}
