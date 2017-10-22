using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionDomainName : DHCPOption
    {
        public string DomainName { get; set; }

        public DHCPOptionDomainName(string domainName)
        {
            DomainName = domainName;
        }

        public DHCPOptionDomainName(int optionLength, byte[] buffer, long offset)
        {
            DomainName = Encoding.ASCII.GetString(buffer, Convert.ToInt32(offset), optionLength);
        }

        public override string ToString()
        {
            return "Domain name - " + DomainName;
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeASCII(stream, DHCPOptionType.DomainName, DomainName);
        }
    }
}
