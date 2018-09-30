using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Models
{
    public class Message
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int Status { get; set; }
    }
}
