using System;
using System.Collections.Generic;
using System.Text;

namespace LibDHCPServer
{
    public abstract class ClientHardwareAddress
    {
        public virtual HardwareAddressType AddressType { get { return HardwareAddressType.Reserved; } }
        public abstract int AddressLength { get; }
        public abstract byte[] GetBytes();
    }
}
