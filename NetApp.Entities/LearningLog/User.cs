using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.LearningLog
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime DataJoined { get; set; }
    }
}
