using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionNetBIOSOverTCPIPNameServer : DHCPOption
    {
        // TODO : Deep copy on access
        public List<IPAddress> NameServers { get; set; } = new List<IPAddress>();

        public DHCPOptionNetBIOSOverTCPIPNameServer(List<IPAddress> nameServers)
        {
            NameServers = nameServers;
        }

        public DHCPOptionNetBIOSOverTCPIPNameServer(int optionLength, byte[] buffer, long offset)
        {
            NameServers = ReadIPAddresses(buffer, offset, optionLength);
        }

        public override string ToString()
        {
            return "NetBIOS over TCP/IP name servers : " + string.Join(',', NameServers.Select(x => x.ToString()).ToList());
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddressList(stream, DHCPOptionType.NETBIOSNameSrv, NameServers);
        }
    }
}
