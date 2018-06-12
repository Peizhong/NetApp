using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetApp.Entities.LearningLog
{
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; }

        public int TopicId { get; set; }
    }
}
