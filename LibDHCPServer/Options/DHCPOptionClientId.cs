using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
