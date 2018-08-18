﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetApp.Entities.Interfaces;

namespace NetApp.Entities.Mall
{
    public class Product : IQuery
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string Name { get; set; }

        public int DataStatus { get; set; }

        [NotMapped]
        public string ProductId
        {
            get { return Id; }
            set { Id = value; }
        }

        [NotMapped]
        public string ProductName
        {
            get { return Name; }
            set { Name = value; }
        }

        public string Description { get; set; }

        public string ManufacturerId { get; set; }

        public string VendorId { get; set; }

        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Remark { get; set; }
    }
}