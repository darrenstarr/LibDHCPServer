using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionRootPath : DHCPOption
    {
        public string RootPath { get; set; }

        public DHCPOptionRootPath(string rootPath)
        {
            RootPath = rootPath;
        }

        public DHCPOptionRootPath(int optionLength, byte[] buffer, long offset)
        {
            RootPath = Encoding.ASCII.GetString(buffer, Convert.ToInt32(offset), optionLength);
        }

        public override string ToString()
        {
            return "Root path - " + RootPath;
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeASCII(stream, DHCPOptionType.Hostname, RootPath);
        }
    }
}
