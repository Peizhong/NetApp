using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Mall
{
    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        Delivered = 3,
        Returned = 4,
    }

    public class Order
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        
        public string TransactionId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal ActualPrice { get; set; }

        public List<OrderDetail> Details { get; set; }
    }
}