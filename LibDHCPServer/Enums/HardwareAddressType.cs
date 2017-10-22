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


namespace LibDHCPServer.Enums
{
    public enum HardwareAddressType
    {
        Reserved = 0,               // GenericClientHardwareAddress
        Ethernet = 1,               // EthernetClientHardwareAddress
        ExperimentalEthernet = 2,
        AX25 = 3,
        ProteonProNETTokenRing = 4,
        Chaos = 5,
        IEEE802Networks = 6,
        ARCNET = 7,
        Hyperchannel = 8,
        Lanstar = 9,
        AutonetShortAddress = 10,
        LocalTalk = 11,
        LocalNet = 12,
        Ultralink = 13,
        SMDS = 14,
        FrameRelay = 15,
        AsynchronousTransmissionMode = 16,
        HDLC = 17,
        FibreChannel = 18,
        AsynchronousTransmissionMode1 = 19,
        SerialLine = 20,
        AsynchronousTransmissionMode2 = 21,
        MILSTD188220 = 22,
        Metricom = 23,
        IEEE1394_1995 = 24,
        MAPOS = 25,
        Twinaxial = 26,
        EUI64 = 27,
        HIPARP = 28,
        IPandARPoverISO78163 = 29,
        ARPSec = 30,
        IPsectunnel = 31,
        InfiniBand = 32,
        TIA102Project25CommonAirInterface = 33,
        WiegandInterface = 34,
        PureIP = 35,
        HWEXP1 = 36,
        HFI = 37,
        HWEXP2 = 256,
        AEthernet = 257,
    }
}
