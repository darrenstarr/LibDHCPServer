using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionRebindingTimeValue : DHCPOption
    {
        public TimeSpan RebindingTime { get; set; }

        public DHCPOptionRebindingTimeValue(TimeSpan rebindingTime)
        {
            RebindingTime = rebindingTime;
        }

        public DHCPOptionRebindingTimeValue(int optionLength, byte[] buffer, long offset)
        {
            var seconds = Read32UnsignedBE(buffer, offset);
            RebindingTime = TimeSpan.FromSeconds(seconds);
        }

        public override string ToString()
        {
            return "Rebinding time : " + RebindingTime.ToString();
        }
        public override Task Serialize(Stream stream)
        {
            return SerializeTimeSpan(stream, DHCPOptionType.RebindingTime, RebindingTime);
        }
    }
}
