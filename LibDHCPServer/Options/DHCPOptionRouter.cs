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
    public class DHCPOptionRouter : DHCPOption
    {
        // TODO : Deep copy on access
        public List<IPAddress> Routers { get; set; } = new List<IPAddress>();

        public DHCPOptionRouter(List<IPAddress> routers)
        {
            Routers = routers;
        }

        public DHCPOptionRouter(int optionLength, byte[] buffer, long offset)
        {
            Routers = ReadIPAddresses(buffer, offset, optionLength);
        }

        public override string ToString()
        {
            return "Routers : " + string.Join(',', Routers.Select(x => x.ToString()).ToList());
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddressList(stream, DHCPOptionType.Router, Routers);
        }
    }
}
