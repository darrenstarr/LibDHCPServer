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
using LibDHCPServer.HardwareAddressTypes;
using LibDHCPServer.Options;
using System;
using System.Collections.Generic;
using System.Net;

namespace LibDHCPServer
{
    public class DHCPPacket
    {
        public const UInt32 DHCPMagicNumber = 0x63825363;

        public MessageOpCode op { get; set; }
        public HardwareAddressType htype { get; set; }
        public int hlen { get; set; }
        public int hops { get; set; }
        public UInt32 xid { get; set; }
        public int secs { get; set; }
        public int flags { get; set; }
        public IPAddress ciaddr { get; set; }
        public IPAddress yiaddr { get; set; }
        public IPAddress siaddr { get; set; }
        public IPAddress giaddr { get; set; }
        public ClientHardwareAddress chaddr { get; set; }
        public string sname { get; set; }
        public string file { get; set; }
        public UInt32 magicNumber { get; set; }
        public List<DHCPOption> options { get; set; } = new List<DHCPOption>();
    };
}
