# LibDHCPServer

Conscia Norway AS
Author Darren R. Starr
License - MIT but will likely be relicensed to Apache once legal goes at it.

## Introduction

This library contains a packet parser and generator for DHCP packets written in C# for .NET. It is not intended as a
full DHCP server implementation. The code in this project is intended for use as a component in other projects. While
the author of the library does not need most of the TFTP options which have been implemented, it seemed pointless to
release the library with less options.

The library is implemented entirely to use the async programming patterns of .NET and at this time is a .NET Core 2.0
library only. There should be no reason why it would not work with other .NET libraries with a new project file.

Note : This library was written in a period of a few days in order to reach a deadline. While the code itself is "clean",
there are no unit tests and the code has only been tested through Wireshark and with a Cisco 2960 switch as a client device.
All options (except option 43) has been tested by creating packets and reading them back again. Option 43 will probably need
to be rearchitected a bit as Option 60 (vendor class id) is not consistant across different devices from the same vendor. As
Option 60 is intended to be a byte array, I have not used regular expressions to identify the right groupings for option 43.
Instead, to make it work nicely, I'll need to come up with a better matching system for vendor specific options. This will
not be a priority for me.

Note 2 : The library is meant to be started and left running. This is horrifying and should have a stop feature which
employs a cancellation token in server to start and stop the process.

## Structure

### Class - DHCPServer

The entire library runs from the Server class. This is a class which instantiates a socket and listens for incoming DHCP requests.
As of the time of this writing, it is intended to be used strictly with DHCP relays and specifically rejects broadcast packets. This
may change in a later version, but there are no current use cases for supporting broadcast.

### Class - DHCPPacketView

The primary entry point for reading and writing DHCP packets. PacketView is a "MVC" style view into the packet structure. While
the library does not follow MVC strictly, the PacketView is intended to be the entry point for reading and writing all packets.

### Class - DHCPPacketParser

PacketParser reads the raw byte data of a DHCP packet and returns a structured representation that does not retain raw byte data.

PacketParser handles option 60 and 43 "intelligently" in the sense that if 60 comes before 43, then parsing happens as expected.
However if option 43 comes before Option 60, then raw byte data is stored until option 60 presents itself.

### Class - DHCPPacket

The model for containing all the contents of a DHCP packet. This is not a byte representation and although it retains the names
of the fields specified by the RFC, it does not keep byte alignment or packing for "C" style copies. Instead, it is populated
by DHCPPacketParser and written by DHCPPacketView.

### Class - Options.DHCPOption

The base class for all DHCP options. It is responsible for parsing and serializing byte data for DHCP options as well. These
options are meant to be used from within DHCPPacketParser and DHCPPacketView.

### Class - Options.VendorSpecificInformation.DHCPVendorSpecificInformation

The base class for all DHCP Option 43 sub-options. These suboptions are enumerated based on Code and VendorClassIdentifier. 
This is very preliminary code and isn't properly tested at this time.

