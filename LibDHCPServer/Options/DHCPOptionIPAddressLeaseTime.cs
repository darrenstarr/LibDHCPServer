using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionIPAddressLeaseTime : DHCPOption
    {
        public TimeSpan LeaseTime { get; set; }

        public DHCPOptionIPAddressLeaseTime(TimeSpan leaseTime)
        {
            LeaseTime = leaseTime;
        }

        public DHCPOptionIPAddressLeaseTime(int optionLength, byte[] buffer, long offset)
        {
            var seconds = Read32UnsignedBE(buffer, offset);
            LeaseTime = TimeSpan.FromSeconds(seconds);
        }

        public override string ToString()
        {
            return "IP address lease time : " + LeaseTime.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeTimeSpan(stream, Enums.DHCPOptionType.AddressTime, LeaseTime);
        }
    }
}
