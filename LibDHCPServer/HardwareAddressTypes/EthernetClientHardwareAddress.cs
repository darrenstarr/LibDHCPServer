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
using System;
using System.Linq;

namespace LibDHCPServer.HardwareAddressTypes
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
