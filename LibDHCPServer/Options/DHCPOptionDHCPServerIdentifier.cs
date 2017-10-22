using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionDHCPServerIdentifier : DHCPOption
    {
        public IPAddress ServerIdentifier { get; set; } = IPAddress.Any;

        public DHCPOptionDHCPServerIdentifier(IPAddress serverIdentifier)
        {
            ServerIdentifier = serverIdentifier;
        }

        public DHCPOptionDHCPServerIdentifier(int optionLength, byte[] buffer, long offset)
        {
            ServerIdentifier = ReadIPAddress(buffer, offset);
        }

        public override string ToString()
        {
            return "DHCP server identifier : " + ServerIdentifier.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddress(stream, DHCPOptionType.DHCPServerId, ServerIdentifier);
        }
    }
}
