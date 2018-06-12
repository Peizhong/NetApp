using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.LearningLog
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime DataJoined { get; set; }

        public List<Topic> Topics { get; set; }
    }
}
