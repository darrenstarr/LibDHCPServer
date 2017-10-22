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
    public class DHCPOptionNTPServer : DHCPOption
    {
        // TODO : Deep copy
        public List<IPAddress> NTPServers { get; set; } = new List<IPAddress>();

        public DHCPOptionNTPServer(List<IPAddress> ntpServers)
        {
            NTPServers = ntpServers;
        }

        public DHCPOptionNTPServer(int optionLength, byte[] buffer, long offset)
        {
            NTPServers = ReadIPAddresses(buffer, offset, optionLength);
        }

        public override string ToString()
        {
            return "NTP servers : " + string.Join(',', NTPServers.Select(x => x.ToString()).ToList());
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddressList(stream, DHCPOptionType.NTPServers, NTPServers);
        }
    }
}
