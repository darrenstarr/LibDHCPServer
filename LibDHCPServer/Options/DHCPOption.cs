using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public abstract class DHCPOption
    {
        public abstract Task Serialize(Stream stream);

        static protected int Read32SignedBE(byte[] buffer, long offset)
        {
            return
                (Convert.ToInt32(buffer[offset + 0]) << 24) |
                (Convert.ToInt32(buffer[offset + 1]) << 16) |
                (Convert.ToInt32(buffer[offset + 2]) << 8) |
                (Convert.ToInt32(buffer[offset + 3]));
        }

        static protected UInt32 Read32UnsignedBE(byte[] buffer, long offset)
        {
            return
                (Convert.ToUInt32(buffer[offset + 0]) << 24) |
                (Convert.ToUInt32(buffer[offset + 1]) << 16) |
                (Convert.ToUInt32(buffer[offset + 2]) << 8) |
                (Convert.ToUInt32(buffer[offset + 3]));
        }

        static protected UInt32 Read16UnsignedBE(byte[] buffer, long offset)
        {
            return
                (Convert.ToUInt32(buffer[offset + 0]) << 8) |
                (Convert.ToUInt32(buffer[offset + 1]));
        }

        static protected IPAddress ReadIPAddress(byte[] buffer, long offset)
        {
            var addressBuffer = new byte[4];
            Array.Copy(buffer, offset, addressBuffer, 0, 4);
            return new IPAddress(addressBuffer);
        }

        static protected List<IPAddress> ReadIPAddresses(byte[] buffer, long offset, long length)
        {
            if ((length % 4) != 0)
                throw new System.ArgumentException("Length is not divisible by 4, this is not a valid IP address list");

            var result = new List<IPAddress>();
            for (var i = 0; i < length; i += 4)
                result.Add(ReadIPAddress(buffer, offset + i));

            return result;
        }

        static protected void WriteInt32(int value, ref byte[] buffer, int offset)
        {
            buffer[offset + 0] = Convert.ToByte((value >> 24) & 0xFF);
            buffer[offset + 1] = Convert.ToByte((value >> 16) & 0xFF);
            buffer[offset + 2] = Convert.ToByte((value >> 8) & 0xFF);
            buffer[offset + 3] = Convert.ToByte(value & 0xFF);
        }

        static protected void WriteUInt32(UInt32 value, ref byte[] buffer, int offset)
        {
            buffer[offset + 0] = Convert.ToByte(value >> 24);
            buffer[offset + 1] = Convert.ToByte((value >> 16) & 0xFF);
            buffer[offset + 2] = Convert.ToByte((value >> 8) & 0xFF);
            buffer[offset + 3] = Convert.ToByte(value & 0xFF);
        }

        static protected void WriteUInt16(UInt32 value, ref byte[] buffer, int offset)
        {
            buffer[offset + 0] = Convert.ToByte((value >> 8) & 0xFF);
            buffer[offset + 1] = Convert.ToByte(value & 0xFF);
        }

        protected Task SerializeIPAddress(Stream stream, DHCPOptionType optionType, IPAddress address)
        {
            var buffer = new byte[]
            {
                Convert.ToByte(optionType),
                4,
                0, 0, 0, 0
            };
            Array.Copy(address.GetAddressBytes(), 0, buffer, 2, 4);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        protected Task SerializeIPAddressList(Stream stream, DHCPOptionType optionType, List<IPAddress> addresses)
        {
            var buffer = new byte[2 + (4 * addresses.Count)];
            buffer[0] = Convert.ToByte(optionType);
            buffer[1] = Convert.ToByte(4 * addresses.Count);
            for(var i=0; i<addresses.Count; i++)
                Array.Copy(addresses[i].GetAddressBytes(), 0, buffer, 2 + (i * 4), 4);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        static protected Task SerializeTimeSpan(Stream stream, DHCPOptionType optionType, TimeSpan timeSpan)
        {
            return SerializeInt32(stream, optionType, Convert.ToInt32(timeSpan.TotalSeconds));
        }

        static protected Task SerializeUInt8(Stream stream, DHCPOptionType optionType, byte value)
        {
            var buffer = new byte[] {
                Convert.ToByte(optionType),
                1,
                value
            };

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        static protected Task SerializeUInt16(Stream stream, DHCPOptionType optionType, UInt16 value)
        {
            var buffer = new byte[] {
                Convert.ToByte(optionType),
                2,
                0,0
            };

            WriteUInt16(value, ref buffer, 2);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        static protected Task SerializeUInt32(Stream stream, DHCPOptionType optionType, UInt32 value)
        {
            var buffer = new byte[] {
                Convert.ToByte(optionType),
                4,
                0,0,0,0
            };

            WriteUInt32(value, ref buffer, 2);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        static protected Task SerializeInt32(Stream stream, DHCPOptionType optionType, Int32 value)
        {
            var buffer = new byte[] {
                Convert.ToByte(optionType),
                4,
                0,0,0,0
            };

            WriteInt32(value, ref buffer, 2);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        static protected Task SerializeASCII(Stream stream, DHCPOptionType optionType, string value)
        {
            var buffer = new byte[2 + value.Length];
            buffer[0] = Convert.ToByte(optionType);
            buffer[1] = Convert.ToByte(value.Length);

            var encoded = Encoding.ASCII.GetBytes(value);
            Array.Copy(encoded, 0, buffer, 2, encoded.Length);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        protected Task SerializeBytes(Stream stream, DHCPOptionType optionType, byte[] value)
        {
            var buffer = new byte[2 + value.Length];
            buffer[0] = Convert.ToByte(optionType);
            buffer[1] = Convert.ToByte(2 + value.Length);

            Array.Copy(value, 0, buffer, 2, value.Length);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
