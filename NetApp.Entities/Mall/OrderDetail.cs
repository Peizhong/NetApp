using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Mall
{
    public class OrderDetail
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
