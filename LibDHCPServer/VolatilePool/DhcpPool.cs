using libnetworkutility;
using System;
using System.Collections.Generic;
using System.Net;

namespace LibDHCPServer.VolatilePool
{
    public class DhcpPool
    {
        public NetworkPrefix Network { get; set; }
        public List<IPRange> Exclusions { get; set; }
        public List<IPAddress> DefaultGateways { get; set; }
        public LeaseOptions PoolOptions { get; set; }
        public TimeSpan LeaseDuration { get; set; }
        public TimeSpan RequestTimeOut { get; set; }
        public int MaxIncompleteRequests { get; set; }
    }
}
