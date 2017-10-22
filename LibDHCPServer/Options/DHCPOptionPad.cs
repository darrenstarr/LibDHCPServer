using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionPad : DHCPOption
    {
        public byte[] Data { get; set; }

        public DHCPOptionPad(int length)
        {
            Data = Enumerable.Repeat((byte)0, length).ToArray();
        }

        public DHCPOptionPad(int optionLength, byte[] buffer, long offset)
        {
            if (buffer.Select(x => Convert.ToInt64(x)).Sum() > 0)
                throw new ArgumentException("The provided buffer to DHCPOptionPad contains values other than 0");

            Data = new byte[optionLength];
            Array.Copy(buffer, offset, Data, 0, optionLength);
        }

        public override string ToString()
        {
            return "Pad - " + Data.Length.ToString() + " bytes";
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeBytes(stream, Enums.DHCPOptionType.Pad, Data);
        }
    }
}
