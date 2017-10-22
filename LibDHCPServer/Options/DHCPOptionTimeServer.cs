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
    public class DHCPOptionTimeServer : DHCPOption
    {
        // TODO : Deep copy
        public List<IPAddress> TimeServers { get; set; } = new List<IPAddress>();

        public DHCPOptionTimeServer(List<IPAddress> timeServers)
        {
            TimeServers = timeServers;
        }

        public DHCPOptionTimeServer(int optionLength, byte[] buffer, long offset)
        {
            TimeServers = ReadIPAddresses(buffer, offset, optionLength);
        }

        public override string ToString()
        {
            return "Time servers : " + string.Join(',', TimeServers.Select(x => x.ToString()).ToList());
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddressList(stream, DHCPOptionType.TimeServer, TimeServers);
        }
    }
}
