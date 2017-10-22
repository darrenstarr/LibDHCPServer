using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;

namespace LibDHCPServer
{
    public class Server
    {
        private const int MAX_BUFFER_SIZE = 16384;

        IPEndPoint listenerEndpoint;
        UdpClient listener;

        public async Task<bool> Start()
        {
            try
            {
                listenerEndpoint = new IPEndPoint(IPAddress.Any, 67);
                listener = new UdpClient(listenerEndpoint);
                //listener.Client.ReceiveTimeout = 500;

                while (true)
                {
                    // TODO : Add task for cancel event
                    var readTask = listener.ReceiveAsync();
                    var completedTask = await Task.WhenAny(readTask).ConfigureAwait(false);

                    if (completedTask == readTask)
                    {
                        var readResult = readTask.Result;
                        var buffer = readResult.Buffer;
                        var remoteEndpoint = readResult.RemoteEndPoint;


                        await ProcessReceivedDHCPPacket(buffer, remoteEndpoint);
                    }
                    else
                    {
                        return false;
                    }
                }
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }

            //return true;
        }

        public async Task<bool> ProcessReceivedDHCPPacket(byte [] buffer, IPEndPoint remoteEndPoint)
        {
            System.Diagnostics.Debug.WriteLine("Buffer received from " + remoteEndPoint.ToString() + " with length " + buffer.Length);

            try
            {
                var packetView = new PacketView(buffer);

                if(packetView.RelayAgentIP.Equals(IPAddress.Any))
                {
                    System.Diagnostics.Debug.WriteLine("Ignoring packet. Only relayed packets accepted.");
                    return true;
                }

                var serverIPAddress = QueryRoutingInterface(packetView.RelayAgentIP);

                System.Diagnostics.Debug.WriteLine("Received DHCP packet via relay : " + packetView.Packet.giaddr.ToString());
                System.Diagnostics.Debug.WriteLine("  Hostname : " + packetView.Hostname);
                System.Diagnostics.Debug.WriteLine("  Local IP facing relay : " + serverIPAddress);

                if(packetView.DHCPMessageType == Enums.DHCPMessageType.DHCPDISCOVER)
                {
                    var offer = new PacketView(Enums.DHCPMessageType.DHCPOFFER);
                    offer.TransactionId = packetView.TransactionId;
                    offer.ClientId = packetView.ClientId;
                    offer.TimeElapsed = packetView.TimeElapsed;

                    offer.RelayAgentIP = packetView.RelayAgentIP;
                    offer.NextServerIP = serverIPAddress;
                    offer.ClientIP = packetView.ClientIP;

                    offer.ClientHardwareAddress = packetView.ClientHardwareAddress;

                    offer.RenewalTimeValue = TimeSpan.FromMinutes(30);
                    offer.RebindingTimeValue = TimeSpan.FromMinutes(45);
                    offer.IPAddressLeaseTime = TimeSpan.FromMinutes(60);
                    offer.DHCPServerIdentifier = serverIPAddress;

                    offer.Hostname = "bob";
                    offer.DomainName = "minions.com";
                    offer.YourIP = IPAddress.Parse("172.20.0.10");
                    offer.BroadcastAddress = IPAddress.Parse("172.20.0.255");
                    offer.SubnetMask = IPAddress.Parse("255.255.255.0");
                    offer.Routers = new List<IPAddress>{ IPAddress. Parse("172.20.0.1") };
                    offer.DomainNameServers = new List<IPAddress> { IPAddress.Parse("10.100.11.81"), IPAddress.Parse("10.100.11.82") };
                    //offer.TimeServers = new List<IPAddress> { IPAddress.Parse("10.100.1.1") };
                    offer.TimeOffset = TimeSpan.FromHours(-2);
                    offer.TFTPServer = "files.minions.com";
                    offer.TFTPBootfile = "config.txt";
                    //offer.SetRelayAgentCircuitId("bobthebuilder");
                    offer.RelayAgentRemoteId = new byte[] { 1, 2, 3, 4, 5 };
                    offer.NTPServers = new List<IPAddress> { IPAddress.Parse("10.100.1.1") };
                    offer.AddClasslessStaticRoute(IPAddress.Parse("10.0.0.0"), 16, IPAddress.Parse("172.20.0.14"));
                    offer.AddClasslessStaticRoute(IPAddress.Parse("192.168.2.0"), 23, IPAddress.Parse("172.20.0.15"));
                    offer.DHCPMessage = "Testing 1-2-3";
                    offer.AddressRequest = IPAddress.Parse("1.2.3.4");

                    var offerBuffer = await offer.GetBytes();

                    var feedback = new PacketView(offerBuffer);

                    System.Diagnostics.Debug.WriteLine("Prepared DHCP offer");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to parse packet : " + e.Message);
                return false;
            }

            return true;
        }

        private Dictionary<IPAddress, IPAddress> RouteCache = new Dictionary<IPAddress, IPAddress>();

        private IPAddress QueryRoutingInterface(
            IPAddress remoteIp)
        {
            IPAddress result;
            if (!RouteCache.TryGetValue(remoteIp, out result))
            {
                var remoteEndPoint = new IPEndPoint(remoteIp, 0);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                result = QueryRoutingInterface(socket, remoteEndPoint).Address;
                RouteCache.Add(remoteIp, result);
            }
            return result;
        }

        private IPEndPoint QueryRoutingInterface(
                  Socket socket,
                  IPEndPoint remoteEndPoint)
        {
            var socketAddress = remoteEndPoint.Serialize();

            var remoteAddressBytes = new byte[socketAddress.Size];
            for (int i = 0; i < socketAddress.Size; i++)
                remoteAddressBytes[i] = socketAddress[i];

            var outBytes = new byte[remoteAddressBytes.Length];
            socket.IOControl(IOControlCode.RoutingInterfaceQuery, remoteAddressBytes, outBytes);

            for (int i = 0; i < socketAddress.Size; i++)
                socketAddress[i] = outBytes[i];

            return (IPEndPoint)remoteEndPoint.Create(socketAddress);
        }
    }
}
