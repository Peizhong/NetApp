using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.LearningLog
{
    public class Topic
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateTime { get; set; }

        public string OwnerId { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
