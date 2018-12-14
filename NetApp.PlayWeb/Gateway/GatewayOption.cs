using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public class DownStream
    {
        public string Host { get; set; }

        public string Port { get; set; }
    }

    public class Route
    {
        public string ServiceName { get; set; }

        public List<string> UpstreamHttpMethod { get; set; }

        public string UpstreamPathTemplate { get; set; }

        public string DownstreamPathTemplate { get; set; }

        public List<DownStream> DownstreamHostAndPorts { get; set; }

        /// <summary>
        /// 缓存
        /// </summary>
        public int? CacheTtlSeconds { get; set; }
    }

    public class GatewayOption
    {
        public string Author { get; set; }

        public List<Route> Routes { get; set; }
    }
}
