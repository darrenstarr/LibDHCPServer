using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibDHCPServer
{
    public class GenericClientHardwareAddress : ClientHardwareAddress
    {
        public byte[] HardwareAddress;
        public GenericClientHardwareAddress(byte [] buffer, long offset, long length)
        {
            HardwareAddress = new byte[length];
            Array.Copy(buffer, offset, HardwareAddress, 0, length);
        }

        public override int AddressLength
        {
            get { return HardwareAddress.Length; }
        }

        public override string ToString()
        {
            if (Encoding.ASCII.GetChars(HardwareAddress, 0, HardwareAddress.Length).Select(x => Char.IsControl(x)).Where(x => x).FirstOrDefault())
                return "Generic - " + String.Join(",", (HardwareAddress.Select(x => x.ToString("X2"))));
            else
                return "Generic - " + Encoding.ASCII.GetString(HardwareAddress);
        }

        public override byte[] GetBytes()
        {
            return HardwareAddress;
        }
    }
}
