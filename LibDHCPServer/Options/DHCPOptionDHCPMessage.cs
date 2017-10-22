using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionDHCPMessage : DHCPOption
    {
        public string Message { get; set; }

        public DHCPOptionDHCPMessage(string message)
        {
            Message = message;
        }

        public DHCPOptionDHCPMessage(int optionLength, byte[] buffer, long offset)
        {
            Message = Encoding.ASCII.GetString(buffer, Convert.ToInt32(offset), optionLength);
        }

        public override string ToString()
        {
            return "DHCP message - " + Message;
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeASCII(stream, DHCPOptionType.DHCPMessage, Message);
        }
    }
}
