using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetApp.Models.Abstractions;
using Newtonsoft.Json;
using ProtoBuf;

namespace NetApp.Models
{
    [ProtoContract]
    public class Product : IBase
    {
        [ProtoMember(1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ProtoMember(2)]
        [Required]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int DataStatus { get; set; }

        [NotMapped]
        public string ProductId => Name;

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

        public static Product FromJson(string json)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Product>(json);
                return result;
            }
            catch {; }
            return null;
        }
    }
}