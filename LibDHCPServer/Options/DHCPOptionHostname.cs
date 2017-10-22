using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionHostname : DHCPOption
    {
        public string Hostname { get; set; }

        public DHCPOptionHostname(string hostname)
        {
            Hostname = hostname;
        }

        public DHCPOptionHostname(int optionLength, byte[] buffer, long offset)
        {
            Hostname = Encoding.ASCII.GetString(buffer, Convert.ToInt32(offset), optionLength);
        }

        public override string ToString()
        {
            return "Hostname - " + Hostname;
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeASCII(stream, DHCPOptionType.Hostname, Hostname);
        }
    }
}
