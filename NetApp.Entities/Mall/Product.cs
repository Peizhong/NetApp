using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetApp.Entities.Interfaces;

namespace NetApp.Entities.Mall
{
    public class Product : IQuery
    {
        [NotMapped]
        public string Id { get; set; }

        [NotMapped]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string ManufacturerId { get; set; }

        public string VendorId { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        public DateTime? UpdateTime { get; set; }

        public string Remark { get; set; }
    }

    public class Product2 : IQuery
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }
    }
}