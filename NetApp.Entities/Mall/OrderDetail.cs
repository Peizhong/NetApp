using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Mall
{
    public class OrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderDetailId { get; set; }
        
        public Order Order { get; set; }
        
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
