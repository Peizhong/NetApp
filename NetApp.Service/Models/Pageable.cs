using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetApp.Entities.Interfaces;

namespace NetApp.Service.Models
{
    public class Pageable
    {
        public int PageNo { get; set; } = 0;

        public int PageSize { get; set; } = 20;

        [JsonIgnore]
        public int StartIndex => PageNo * PageSize;
    }

    public class Filterable<T> : Pageable where T : IQuery, new()
    {
        public T Source { get; set; }
    }

    public class ProductViewModel : Pageable
    {
        public string CategoryId { get; set; }
    }
}