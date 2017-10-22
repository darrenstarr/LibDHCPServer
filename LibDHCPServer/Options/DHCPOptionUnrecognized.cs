using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionUnrecognized : DHCPOption
    {
        DHCPOptionType OptionNumber;
        public byte[] Data { get; set; }

        public DHCPOptionUnrecognized(int optionNumber, int optionLength, byte [] buffer, long offset)
        {
            OptionNumber = (DHCPOptionType)optionNumber;
            Data = new byte[optionLength];
            Array.Copy(buffer, offset, Data, 0, optionLength);
        }

        public override string ToString()
        {
            return "Unrecognized - " + OptionNumber.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            var buffer = new byte[2 + Data.Length];

            buffer[0] = Convert.ToByte(OptionNumber);
            buffer[1] = Convert.ToByte(Data.Length);
            Array.Copy(Data, 0, buffer, 2, Data.Length);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
