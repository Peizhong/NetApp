using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Mall
{
    public class Product
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ManufacturerId { get; set; }
        public string VendorId { get; set; }
        public decimal Price { get; set; }
    }
}