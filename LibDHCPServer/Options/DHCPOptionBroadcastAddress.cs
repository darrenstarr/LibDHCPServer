using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionBroadcastAddress : DHCPOption
    {
        public IPAddress BroadcastAddress { get; set; } = IPAddress.Any;

        public DHCPOptionBroadcastAddress(IPAddress broadcastAddress)
        {
            BroadcastAddress = broadcastAddress;
        }

        public DHCPOptionBroadcastAddress(int optionLength, byte[] buffer, long offset)
        {
            BroadcastAddress = ReadIPAddress(buffer, offset);
        }

        public override string ToString()
        {
            return "Broadcast address : " + BroadcastAddress.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddress(stream, DHCPOptionType.BroadcastAddress, BroadcastAddress);
        }
    }
}
