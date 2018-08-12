using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace NetApp.Entities.Mall
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CategoryId { get; set; }

        public string ParentCategoryId { get; set; }

        [JsonIgnore]
        public Category ParentCategory { get; set; }

        public int CategoryType { get; set; }

        public string CategoryName { get; set; }

        public string FullPath { get; set; }

        public int IsShow { get; set; }

        public double? SortNo { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Remark { get; set; }

        [JsonIgnore]
        public ICollection<Category> Categories { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}