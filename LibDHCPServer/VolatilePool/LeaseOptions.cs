using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LibDHCPServer.VolatilePool
{
    public class LeaseOptions
    {
        public string DomainName { get; set; }
        public List<IPAddress> DNSServers { get; set; }
        public List<string> TFTPServers { get; set; }
        public string BootFile { get; set; }
        public string Hostname { get; set; }
    }
}
