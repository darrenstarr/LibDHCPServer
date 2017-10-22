using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionSubnetMask : DHCPOption
    {
        public IPAddress SubnetMask { get; set; } = IPAddress.Any;

        public DHCPOptionSubnetMask(IPAddress subnetMask)
        {
            SubnetMask = subnetMask;
        }

        public DHCPOptionSubnetMask(int optionLength, byte[] buffer, long offset)
        {
            SubnetMask = ReadIPAddress(buffer, offset);
        }

        public override string ToString()
        {
            return "Subnet mask : " + SubnetMask.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddress(stream, DHCPOptionType.SubnetMask, SubnetMask);
        }
    }
}
