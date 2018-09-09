using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NetApp.Common.Abstractions
{
    public class PageableQueryResult<T> : Pageable
    {
        public IList<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int CurrentCount { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public static PageableQueryResult<T> FromJson(string json)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<PageableQueryResult<T>>(json);
                return result;
            }
            catch {; }
            return null;
        }
    }
}