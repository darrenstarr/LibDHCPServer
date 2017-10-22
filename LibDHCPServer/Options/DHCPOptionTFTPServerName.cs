using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionTFTPServerName : DHCPOption
    {
        public string TFTPServerName { get; set; }

        public DHCPOptionTFTPServerName(string tftpServerName)
        {
            TFTPServerName = tftpServerName;
        }

        public DHCPOptionTFTPServerName(int optionLength, byte[] buffer, long offset)
        {
            TFTPServerName = Encoding.ASCII.GetString(buffer, Convert.ToInt32(offset), optionLength);
        }

        public override string ToString()
        {
            return "TFTP Server - " + TFTPServerName;
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeASCII(stream, DHCPOptionType.ServerName, TFTPServerName);
        }
    }
}
