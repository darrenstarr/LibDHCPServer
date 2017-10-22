using System;
using LibDHCPServer;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using LibDHCPServer.Enums;

namespace TestLibDHCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.OnDHCPDiscover += new Server.DHCPProcessDelegate(async (discovery, localEndPoint, remoteEndPoint) =>
            {
                return await GenerateLease(discovery, localEndPoint, remoteEndPoint);
            });

            server.OnDHCPRequest += new Server.DHCPProcessDelegate(async (discovery, localEndPoint, remoteEndPoint) =>
            {
                return await GenerateLease(discovery, localEndPoint, remoteEndPoint);
            });

            Task.Factory.StartNew(() => { server.Start(); });
            Thread.Sleep(600000);
        }

        static private async Task<PacketView> GenerateLease(PacketView discovery, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            PacketView result;
            switch (discovery.DHCPMessageType)
            {
                case DHCPMessageType.DHCPDISCOVER:
                    result = new PacketView(DHCPMessageType.DHCPOFFER);
                    break;
                case DHCPMessageType.DHCPREQUEST:
                    result = new PacketView(DHCPMessageType.DHCPACK);
                    break;
                default:
                    return null;
            }

            result.ClientHardwareAddress = discovery.ClientHardwareAddress;
            result.TransactionId = discovery.TransactionId;
            result.TimeElapsed = discovery.TimeElapsed;
            result.BroadcastFlag = discovery.BroadcastFlag;
            result.NextServerIP = localEndPoint.Address;
            result.RelayAgentIP = discovery.RelayAgentIP;
            result.Hops = discovery.Hops;

            result.RenewalTimeValue = TimeSpan.FromHours(12);
            result.RebindingTimeValue = TimeSpan.FromHours(21);
            result.IPAddressLeaseTime = TimeSpan.FromHours(24);

            result.DHCPServerIdentifier = localEndPoint.Address;

            result.YourIP = IPAddress.Parse("172.20.0.99");
            result.SubnetMask = IPAddress.Parse("255.255.255.0");
            result.Routers = new List<IPAddress> { IPAddress.Parse("172.20.0.1") };
            result.Hostname = "bob";
            result.DomainName = "minions.com";
            result.DomainNameServers = new List<IPAddress> { IPAddress.Parse("10.100.11.81") };

            return result;
        }
    }
}
