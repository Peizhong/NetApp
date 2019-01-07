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

        [NotMapped]
        public DateTime? NextTime { get; set; }

        public DateTime? Expires { get; set; }

        public CrontabSchedule CrontabSchedule { get; set; }

        public IntervalSchedule IntervalSchedule { get; set; }
    }

    public class CrontabSchedule 
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
    }

    public enum EnumPeriod
    {
        Days = 1,
        Hours = 2,
        Minutes = 3,
        Seconds = 4,
    }

    public class IntervalSchedule
    {
        [Key]
        public string Id { get; set; }

        public int Every { get; set; }
        
        public EnumPeriod Period { get; set; }
    }
}
