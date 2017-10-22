# A DHCP Server Library for embedding
by Darren R. Starr - Conscia Norway AS

## Introduction
Included in this repository is a DHCP server library which was developed for the purpose of embedding in another system. 
It's core advantage over using a dedicated DHCP server (such as Windows DHCP) is the ability to directly access local
information when necessary. In this case, I've developed it to integrate as a component of a network management system
which provides IP addresses to devices before they have a configuration present. The information about these devices are
stored in a series of text files and using DHCP Option 82, IP addresses are assigned to devices based on where they are
connected as opposed to using identifying information such as MAC address. This is of course Microsoft DHCP Server has
this functionality as well, but it is very difficult to maintain and requires maintaining in the first place. This DHCP
server is used to pull information directly from the network configuration files instead.

## TODO
* Write better documentation. We're not there yet. I'm still implementing packet parsing and state machines.
* Extend .NET Core 2 with a proper library for network information as the current one is horrible.
 * Add support for querying the routing table for outgoing interface. This will allow for choosing the DHCP server
   address facing the client as the server IP in the DHCP packets.
* Add code for "System.Net.NetworkInformation.NetworkChange" so that the server IP address would change in the packets if 
the network changes.
