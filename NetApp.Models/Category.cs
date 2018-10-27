using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NetApp.Models.Abstractions;

namespace NetApp.Models
{
    public class Category : ITreeNode<Category>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [NotMapped]
        public string CategoryId
        {
            get { return Id; }
            set { Id = value; }
        }

        public string Name { get; set; }

        [NotMapped]
        public string CategoryName
        {
            get { return Name; }
            set { Name = value; }
        }

        public string ParentId { get; set; }

        [JsonIgnore]
        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }

        public int DataStatus { get; set; }

        public int CategoryType { get; set; }

        public string FullPath { get; set; }

        public double? SortNo { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string Remark { get; set; }
    }
}