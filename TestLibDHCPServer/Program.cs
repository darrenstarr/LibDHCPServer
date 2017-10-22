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

using LibDHCPServer;
using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TestLibDHCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new DHCPServer();
            server.OnDHCPDiscover += new DHCPServer.DHCPProcessDelegate(async (discovery, localEndPoint, remoteEndPoint) =>
            {
                return await GenerateLease(discovery, localEndPoint, remoteEndPoint);
            });

            server.OnDHCPRequest += new DHCPServer.DHCPProcessDelegate(async (discovery, localEndPoint, remoteEndPoint) =>
            {
                return await GenerateLease(discovery, localEndPoint, remoteEndPoint);
            });

            Task.Factory.StartNew(() => { server.Start(); });
            Thread.Sleep(600000);
        }

        static private async Task<DHCPPacketView> GenerateLease(DHCPPacketView discovery, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            DHCPPacketView result;
            switch (discovery.DHCPMessageType)
            {
                case DHCPMessageType.DHCPDISCOVER:
                    result = new DHCPPacketView(DHCPMessageType.DHCPOFFER);
                    break;
                case DHCPMessageType.DHCPREQUEST:
                    result = new DHCPPacketView(DHCPMessageType.DHCPACK);
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
