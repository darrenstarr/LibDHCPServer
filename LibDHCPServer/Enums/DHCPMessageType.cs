using System;
using System.Collections.Generic;
using System.Text;

namespace LibDHCPServer.Enums
{
    public enum DHCPMessageType
    {
        Unspecified = -1,
        DHCPDISCOVER = 1,
        DHCPOFFER = 2,
        DHCPREQUEST = 3,
        DHCPDECLINE = 4,
        DHCPACK = 5,
        DHCPNAK = 6,
        DHCPRELEASE = 7,
        DHCPINFORM = 8
    }
}
