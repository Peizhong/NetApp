using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Business.Interfaces.DTO
{
    public class TopicDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class EntryDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
