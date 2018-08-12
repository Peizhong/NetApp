using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetApp.Service.Models
{
    public class Pageable
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 20;

        [JsonIgnore]
        public int StartIndex => PageIndex * PageSize;
    }

    public class ProductViewModel : Pageable
    {
        public string CategoryId { get; set; }
    }
}
