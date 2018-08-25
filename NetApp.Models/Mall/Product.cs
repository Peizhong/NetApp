using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetApp.Models.Interfaces;

namespace NetApp.Models.Mall
{
    public class Product : IBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int DataStatus { get; set; }

        [NotMapped]
        public string ProductId
        {
            get { return Id; }
            set { Id = value; }
        }

        [NotMapped]
        public string ProductName => Name;

        public string Description { get; set; }

        public string ManufacturerId { get; set; }

        public string VendorId { get; set; }

        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Remark { get; set; }
    }
}