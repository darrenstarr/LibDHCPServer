using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibDHCPServer
{
    public class EthernetClientHardwareAddress : ClientHardwareAddress
    {
        public byte[] Address { get; set; } = new byte[] { 0, 0, 0, 0, 0, 0 };

        public override HardwareAddressType AddressType { get { return HardwareAddressType.Ethernet; } }

        public override int AddressLength
        {
            get { return Address.Length; }
        }

        public EthernetClientHardwareAddress(byte [] address)
        {
            if (address.Length < 6)
                throw new ArgumentException("Address must contain at least 6 bytes", "address");

            Array.Copy(address, Address, 6);
        }

        public override string ToString()
        {
            return string.Join(":", Address.ToList().Select(x => Convert.ToUInt32(x).ToString("X2")).ToArray());
        }

        public override byte[] GetBytes()
        {
            return Address;
        }
    }
}
