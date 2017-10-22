using System;
using System.Collections.Generic;
using System.Text;

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
