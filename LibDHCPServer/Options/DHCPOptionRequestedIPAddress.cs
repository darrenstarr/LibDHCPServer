using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionRequestedIPAddress : DHCPOption
    {
        public IPAddress RequestedIP { get; set; } = IPAddress.Any;

        public DHCPOptionRequestedIPAddress(IPAddress requestedIP)
        {
            RequestedIP = requestedIP;
        }

        public DHCPOptionRequestedIPAddress(int optionLength, byte[] buffer, long offset)
        {
            RequestedIP = ReadIPAddress(buffer, offset);
        }

        public override string ToString()
        {
            return "Address request : " + RequestedIP.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddress(stream, DHCPOptionType.AddressRequest, RequestedIP);
        }
    }
}
