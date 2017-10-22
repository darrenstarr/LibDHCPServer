using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionRenewalTimeValue : DHCPOption
    {
        public TimeSpan RenewalTime { get; set; }

        public DHCPOptionRenewalTimeValue(TimeSpan renewalTime)
        {
            RenewalTime = renewalTime;
        }

        public DHCPOptionRenewalTimeValue(int optionLength, byte[] buffer, long offset)
        {
            var seconds = Read32SignedBE(buffer, offset);
            RenewalTime = TimeSpan.FromSeconds(seconds);
        }

        public override string ToString()
        {
            return "Renewal time : " + RenewalTime.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeTimeSpan(stream, DHCPOptionType.RenewalTime, RenewalTime);
        }
    }
}
