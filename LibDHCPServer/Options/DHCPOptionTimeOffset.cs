using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionTimeOffset : DHCPOption
    {
        public TimeSpan TimeOffset { get; set; }

        public DHCPOptionTimeOffset(TimeSpan timeOffset)
        {
            TimeOffset = timeOffset;
        }

        public DHCPOptionTimeOffset(int optionLength, byte[] buffer, long offset)
        {
            var seconds = Read32SignedBE(buffer, offset);
            TimeOffset = TimeSpan.FromSeconds(seconds);
        }

        public override string ToString()
        {
            return "Time offset : " + TimeOffset.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeTimeSpan(stream, DHCPOptionType.TimeOffset, TimeOffset);
        }
    }
}
