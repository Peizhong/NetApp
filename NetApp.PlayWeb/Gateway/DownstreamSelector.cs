using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public interface IDownstreamSelector
    {
        // apply load balance
        DownStream GetHostAndPort(string serivceName, IList<DownStream> downStreams);
    }

    public class DownstreamSelector : IDownstreamSelector
    {
        private readonly ILogger<DownstreamSelector> _logger;

        public DownstreamSelector(ILogger<DownstreamSelector> logger)
        {
            _logger = logger;
        }

        public DownStream GetHostAndPort(string serivceName, IList<DownStream> downStreams)
        {
            var one = downStreams?.FirstOrDefault();
            if (one == null)
                throw new ArgumentNullException("downStreams");
            return one;
        }
    }
}
