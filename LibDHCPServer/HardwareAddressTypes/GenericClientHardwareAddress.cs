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

using System;
using System.Linq;
using System.Text;

namespace LibDHCPServer.HardwareAddressTypes
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
