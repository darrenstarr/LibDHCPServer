/// The MIT License(MIT)
/// 
/// Copyright(c) 2017 Conscia Norway AS
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.

using LibDHCPServer.Enums;
using LibDHCPServer.HardwareAddressTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionClientId : DHCPOption
    {
        public HardwareAddressType IdType { get; set; }
        private ClientHardwareAddress _clientId = null;
        public ClientHardwareAddress ClientId {
            get { return _clientId; }
            set {
                IdType = value.AddressType;
                _clientId = value;
            }
        }

        public DHCPOptionClientId(int optionLength, byte[] buffer, long offset)
        {
            IdType = (HardwareAddressType)buffer[offset];
            switch(IdType)
            {
                case HardwareAddressType.Ethernet:
                    {
                        var ethernetAddress = new byte[6];
                        Array.Copy(buffer, offset + 1, ethernetAddress, 0, 6);
                        _clientId = new EthernetClientHardwareAddress(ethernetAddress);
                    }
                    break;
                default:
                    _clientId = new GenericClientHardwareAddress(buffer, offset + 1, optionLength - 1);
                    break;
            }
        }

        public DHCPOptionClientId(ClientHardwareAddress clientId)
        {
            ClientId = clientId;
        }

        public override string ToString()
        {
            return "ClientId: " + ClientId.ToString();
        }

        public override Task Serialize(Stream stream)
        {
            var addressBuffer = _clientId.GetBytes();
            var buffer = new byte[3 + addressBuffer.Length];
            buffer[0] = Convert.ToByte(DHCPOptionType.ClientId);
            buffer[1] = Convert.ToByte(1 + addressBuffer.Length);
            buffer[2] = Convert.ToByte(IdType);
            Array.Copy(addressBuffer, 0, buffer, 3, addressBuffer.Length);

            return stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
