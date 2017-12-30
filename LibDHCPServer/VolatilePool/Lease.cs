using LibDHCPServer.HardwareAddressTypes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LibDHCPServer.VolatilePool
{
    public class Lease
    {
        public DhcpPool Pool { get; set; }
        public IPAddress Address { get; set; }
        public LeaseOptions Options { get; set; }
        public DateTimeOffset Requested { get; set; }
        public DateTimeOffset Acknowledged { get; set; }
        public DateTimeOffset Renewed { get; set; }
        public DateTimeOffset Expires { get; set; }
        public DateTimeOffset TimesOut { get; set; }
        public ClientHardwareAddress ClientId { get; set; }
        public UInt32 TransactionId { get; set; }
    }
}
