using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionDHCPMessageType : DHCPOption
    {
        public DHCPMessageType MessageType { get; set; }

        public DHCPOptionDHCPMessageType(DHCPMessageType messageType)
        {
            MessageType = messageType;
        }

        public DHCPOptionDHCPMessageType(int optionLength, byte[] buffer, long offset)
        {
            MessageType = (DHCPMessageType)Convert.ToInt32(buffer[offset]);
        }

        public override string ToString()
        {
            return "DHCPMsgType- " + MessageType.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeUInt8(stream, DHCPOptionType.DHCPMsgType, Convert.ToByte(MessageType));
        }
    }
}
