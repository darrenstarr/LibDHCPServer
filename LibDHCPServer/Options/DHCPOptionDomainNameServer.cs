/// The MIT License(MIT)
/// 
/// Copyright(c) 2017 Conscia Norway AS
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.

using LibDHCPServer.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
