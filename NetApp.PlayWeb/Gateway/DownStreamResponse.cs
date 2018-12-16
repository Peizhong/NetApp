using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public class DownStreamResponse
    {
        public DownStreamResponse(
            int statusCode,
            string reasonPhrase,
            Dictionary<string, IEnumerable<string>> headers,
            Dictionary<string, IEnumerable<string>> contentHeaders,
            byte[] contentBytes)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Headers = headers;
            ContentHeaders = contentHeaders;
            ContentBytes = contentBytes;
        }

        public int StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        public Dictionary<string, IEnumerable<string>> ContentHeaders { get; set; }

        public byte[] ContentBytes { get; set; }
    }

    public class CacheResponse : DownStreamResponse, Cacheable
    {
        public CacheResponse(
            string key,
            int statusCode,
            string reasonPhrase,
            Dictionary<string, IEnumerable<string>> headers,
            Dictionary<string, IEnumerable<string>> contentHeaders,
            byte[] contentBytes,
            DateTime expire)
            : base(statusCode, reasonPhrase, headers, contentHeaders, contentBytes)
        {
            Key = key;
            Expire = expire;
        }

        public string Key { get; set; }

        public DateTime Expire { get; set; }
    }
}
