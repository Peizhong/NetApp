using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetApp.PlayASPAPI.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}