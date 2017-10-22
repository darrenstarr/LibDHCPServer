using LibDHCPServer.Enums;
using LibDHCPServer.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer
{
    public class Packet
    {
        public const UInt32 DHCPMagicNumber = 0x63825363;

        public MessageOpCode op { get; set; }
        public HardwareAddressType htype { get; set; }
        public int hlen { get; set; }
        public int hops { get; set; }
        public UInt32 xid { get; set; }
        public int secs { get; set; }
        public int flags { get; set; }
        public IPAddress ciaddr { get; set; }
        public IPAddress yiaddr { get; set; }
        public IPAddress siaddr { get; set; }
        public IPAddress giaddr { get; set; }
        public ClientHardwareAddress chaddr { get; set; }
        public string sname { get; set; }
        public string file { get; set; }
        public UInt32 magicNumber { get; set; }
        public List<DHCPOption> options { get; set; } = new List<DHCPOption>();
    };
}
