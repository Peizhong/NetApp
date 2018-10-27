using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Models
{
    public enum MessageStatus
    {
        Create = 1,
        Edit = 2,
        Approving = 3,
        Approved = 4,
        Reject = 5,
        Complete = 6,
        Cancel = 7
    }

    public class Message
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public MessageStatus Status { get; set; }
    }
}
