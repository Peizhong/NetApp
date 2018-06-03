using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.LearningLog
{
    public class Entry
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public DateTime UpdateTime { get; set; }

        public int TopicId { get; set; }
    }
}
