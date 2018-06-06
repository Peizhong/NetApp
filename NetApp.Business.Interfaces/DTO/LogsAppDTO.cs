using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Business.Interfaces.DTO
{
    public class TopicHeaderDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class TopicDTO : TopicHeaderDTO
    {
        public DateTime UpdateTime { get; set; }

        public IEnumerable<EntryHeaderDTO> EntryHeaders { get; set; }
    }

    public class EntryHeaderDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class EntryDTO : EntryHeaderDTO
    {
        public string Text { get; set; }

        public string Link { get; set; }

        public string UpdateTime { get; set; }

        public int TopicId { get; set; }
    }
}