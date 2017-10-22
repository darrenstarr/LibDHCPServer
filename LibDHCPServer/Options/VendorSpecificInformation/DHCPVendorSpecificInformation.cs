using System;
using System.Collections.Generic;
using System.Text;

namespace LibDHCPServer.Options.VendorSpecificInformation
{
    public abstract class DHCPVendorSpecificInformation
    {
        public abstract byte [] VendorClassIdentifier { get; }

        public abstract byte Code { get; }

        public abstract byte[] Serialize();
    }
}
