using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LibDHCPServer.Utility.RemoteAgentIdFormats
{
    public class CiscoRouterTLV
    {
        public class TLV
        {

        }

        public class UnknownTLV : TLV
        {
            public byte Code { get; set; }
            public byte [] Buffer { get; set; }
        }

        public class Type2TLV : TLV
        {

            public UInt16 Unknown { get; set; }
            public IPAddress DeviceIP { get; set; }
            public UInt32 SNMPInterfaceIndex { get; set; }

            public static Type2TLV Parse(byte [] buffer)
            {
                return new Type2TLV
                {
                    Unknown = ParserTools.Read16UnsignedBE(buffer, 0),
                    DeviceIP = ParserTools.ReadIPAddress(buffer, 2),
                    SNMPInterfaceIndex = ParserTools.Read32UnsignedBE(buffer, 6)
                };
            }
        }

        public List<TLV> TLVs { get; set; } = new List<TLV>();

        public CiscoRouterTLV(byte [] buffer)
        {
            var index = 0;
            while(index < buffer.Length)
            {
                var code = buffer[index++];
                var length = buffer[index++];
                var tlvBuffer = new byte[length];
                Array.Copy(buffer, index, tlvBuffer, 0, length);
                index += length;

                switch(code)
                {
                    case (byte)2:
                        TLVs.Add(Type2TLV.Parse(tlvBuffer));
                        break;
                    default:
                        TLVs.Add(new UnknownTLV { Code = code, Buffer = tlvBuffer });
                        break;
                }
            }
        }

        public UInt32 SNMPInterfaceIndex
        {
            get
            {
                var type2 = (Type2TLV)TLVs.Where(x => x is Type2TLV).FirstOrDefault();
                if (type2 == null)
                    return UInt32.MaxValue;

                return type2.SNMPInterfaceIndex;
            }
        }
    }
}
