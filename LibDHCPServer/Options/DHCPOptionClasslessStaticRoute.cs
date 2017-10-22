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
    public class DHCPOptionClasslessStaticRoute : DHCPOption
    {
        public class NetworkPrefix
        {
            public IPAddress Prefix { get; set; }
            public int Length { get; set; }
        }

        public class RouteEntry
        {
            public NetworkPrefix Prefix { get; set; }
            public IPAddress NextHop { get; set; }
        };

        // TODO : Deep copy
        public List<RouteEntry> Entries { get; set; } = new List<RouteEntry>();

        public DHCPOptionClasslessStaticRoute(List<RouteEntry> entries)
        {
            Entries = entries;
        }

        public DHCPOptionClasslessStaticRoute(int optionLength, byte[] buffer, long offset)
        {
            int index = 0;
            while(index < optionLength)
            {
                var prefixLength = Convert.ToInt32(buffer[index + offset]);
                index++;

                var byteLength = (prefixLength / 8) + 1;
                byte [] addressBuffer = new byte[] { 0, 0, 0, 0 };
                for(var i=0; i<byteLength; i++,index++)
                    addressBuffer[i] = buffer[index + offset];

                var prefixAddress = new IPAddress(addressBuffer);
                var nextHop = ReadIPAddress(buffer, index + offset);
                index += 4;

                Entries.Add(
                    new RouteEntry {
                        Prefix = new NetworkPrefix {
                            Prefix = prefixAddress,
                            Length = prefixLength
                        },
                        NextHop = nextHop
                    });
            }
        }

        public override string ToString()
        {
            return "Classless static routes - " + string.Join(",", Entries.Select(x => "{" + x.Prefix.Prefix.ToString() + "/" + x.Prefix.Length.ToString() + "->" + x.NextHop.ToString() + "}"));
        }

        private byte[] SerializeEntry(RouteEntry entry)
        {
            var byteLength = (entry.Prefix.Length / 8) + 1;
            var result = new byte[1 + byteLength + 4];
            result[0] = Convert.ToByte(entry.Prefix.Length);
            Array.Copy(entry.Prefix.Prefix.GetAddressBytes(), 0, result, 1, byteLength);
            Array.Copy(entry.NextHop.GetAddressBytes(), 0, result, 1 + byteLength, 4);

            return result;
        }

        public override async Task Serialize(Stream stream)
        {
            var encodedEntries = Entries.Select(x => SerializeEntry(x)).ToList();

            var header = new byte[]
            {
                Convert.ToByte(DHCPOptionType.ClasslessStaticRouteOption),
                Convert.ToByte(encodedEntries.Select(x => x.Length).Sum())
            };
            await stream.WriteAsync(header, 0, header.Length);
            foreach(var entry in encodedEntries)
                await stream.WriteAsync(entry, 0, entry.Length);
        }
    }
}
