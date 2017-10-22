using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionTFTPBootfile : DHCPOption
    {
        public string TFTPBootfile { get; set; }

        public DHCPOptionTFTPBootfile(string tftpBootfile)
        {
            TFTPBootfile = tftpBootfile;
        }

        public DHCPOptionTFTPBootfile(int optionLength, byte[] buffer, long offset)
        {
            TFTPBootfile = Encoding.ASCII.GetString(buffer, Convert.ToInt32(offset), optionLength);
        }

        public override string ToString()
        {
            return "TFTP Bootfile - " + TFTPBootfile;
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeASCII(stream, DHCPOptionType.BootfileName, TFTPBootfile);
        }
    }
}
