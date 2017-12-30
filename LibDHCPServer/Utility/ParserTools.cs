using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LibDHCPServer.Utility
{
    class ParserTools
    {
        public static IPAddress ReadIPAddress(byte[] buffer, long offset)
        {
            var addressBuffer = new byte[4];
            Array.Copy(buffer, offset, addressBuffer, 0, 4);
            return new IPAddress(addressBuffer);
        }

        public static UInt32 Read32UnsignedBE(byte[] buffer, long offset)
        {
            return
                (Convert.ToUInt32(buffer[offset + 0]) << 24) |
                (Convert.ToUInt32(buffer[offset + 1]) << 16) |
                (Convert.ToUInt32(buffer[offset + 2]) << 8) |
                (Convert.ToUInt32(buffer[offset + 3]));
        }
        public static UInt16 Read16UnsignedBE(byte[] buffer, long offset)
        {
            return
                Convert.ToUInt16(
                    (Convert.ToUInt32(buffer[offset + 0]) << 8) |
                    (Convert.ToUInt32(buffer[offset + 1]))
                );
        }
    }
}
