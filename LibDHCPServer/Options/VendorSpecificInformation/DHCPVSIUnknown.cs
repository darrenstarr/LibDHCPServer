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

namespace LibDHCPServer.Options.VendorSpecificInformation
{
    public class DHCPVSIUnknown : DHCPVendorSpecificInformation
    {
        public override byte Code
        {
            get { return 0; }
        }

        public override byte[] VendorClassIdentifier
        {
            get { return new byte[0];  }
        }

        public byte ParsedCode { get; set; }

        public byte [] Data { get; set; }

        public DHCPVSIUnknown(byte code, byte [] data)
        {
            ParsedCode = code;
            Data = data;
        }

        public override string ToString()
        {
            return "Unknown (" + Convert.ToString(ParsedCode) + ") length " + Data.Length.ToString();
        }

        public override byte[] Serialize()
        {
            var result = new byte[2 + Data.Length];
            result[0] = Code;
            result[1] = Convert.ToByte(Data.Length);
            Array.Copy(Data, 0, result, 2, Data.Length);

            return result;
        }
    }
}
