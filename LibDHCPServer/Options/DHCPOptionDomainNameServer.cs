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
    public class DHCPOptionDomainNameServer : DHCPOption
    {
        // TODO : Deep copy
        public List<IPAddress> DomainNameServers { get; set; } = new List<IPAddress>();

        public DHCPOptionDomainNameServer(List<IPAddress> domainNameServers)
        {
            DomainNameServers = domainNameServers;
        }

        public DHCPOptionDomainNameServer(int optionLength, byte[] buffer, long offset)
        {
            DomainNameServers = ReadIPAddresses(buffer, offset, optionLength);
        }

        public override string ToString()
        {
            return "Domain name servers : " + string.Join(',', DomainNameServers.Select(x => x.ToString()).ToList());
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeIPAddressList(stream, DHCPOptionType.DomainServer, DomainNameServers);
        }
    }
}
