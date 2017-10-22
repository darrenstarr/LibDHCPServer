using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionMaximumMessageSize : DHCPOption
    {
        public int MaximumMessageSize { get; set; }

        public DHCPOptionMaximumMessageSize(int maximumMessageSize)
        {
            MaximumMessageSize = maximumMessageSize;
        }

        public DHCPOptionMaximumMessageSize(int optionLength, byte[] buffer, long offset)
        {
            MaximumMessageSize = Convert.ToInt32(Read16UnsignedBE(buffer, offset));
        }

        public override string ToString()
        {
            return "Max Message Size - " + MaximumMessageSize.ToString() + " bytes";
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeUInt16(stream, DHCPOptionType.DHCPMaxMsgSize, Convert.ToUInt16(MaximumMessageSize));
        }
    }
}
