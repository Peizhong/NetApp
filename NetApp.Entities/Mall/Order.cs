using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderId { get; set; }
        
        public User User { get; set; }

        public string TransactionId { get; set; }

        public DateTime? CreateTime { get; set; }
        
        public DateTime? UpdateTime { get; set; }

        public OrderStatus Status { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal ActualPrice { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}