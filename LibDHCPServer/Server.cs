using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using LibDHCPServer.Enums;

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

                while (true)
                {
                    // TODO : Add task for cancel event
                    var readTask = listener.ReceiveAsync();
                    var completedTask = await Task.WhenAny(readTask).ConfigureAwait(false);

                    if (completedTask == readTask)
                    {
                        await ProcessReceivedDHCPPacket(readTask.Result.Buffer, readTask.Result.RemoteEndPoint);
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
        }

        public delegate Task<PacketView> DHCPProcessDelegate(PacketView discovery, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint);
        public DHCPProcessDelegate OnDHCPDiscover = null;
        public DHCPProcessDelegate OnDHCPRequest = null;
        public DHCPProcessDelegate OnDHCPRelease = null;

        public async Task<bool> ProcessReceivedDHCPPacket(byte [] buffer, IPEndPoint remoteEndPoint)
        {
            System.Diagnostics.Debug.WriteLine("Buffer received from " + remoteEndPoint.ToString() + " with length " + buffer.Length);

            try
            {
                var request = new PacketView(buffer);

                var serverIPAddress = QueryRoutingInterface(request.RelayAgentIP);
                var localEndPoint = new IPEndPoint(serverIPAddress, 67);

                System.Diagnostics.Debug.WriteLine("Received DHCP packet via relay : " + request.Packet.giaddr.ToString());
                System.Diagnostics.Debug.WriteLine("  Hostname : " + request.Hostname);
                System.Diagnostics.Debug.WriteLine("  Local IP facing relay : " + serverIPAddress);

                if (request.RelayAgentIP.Equals(IPAddress.Any))
                {
                    System.Diagnostics.Debug.WriteLine("Ignoring packet. Only relayed packets accepted.");
                    return true;
                }

                PacketView response = null;
                switch(request.DHCPMessageType)
                {
                    case DHCPMessageType.DHCPDISCOVER:
                        if (OnDHCPDiscover != null)
                            response = await OnDHCPDiscover(request, localEndPoint, remoteEndPoint);
                        break;
                    case DHCPMessageType.DHCPREQUEST:
                        if (OnDHCPRequest != null)
                            response = await OnDHCPRequest(request, localEndPoint, remoteEndPoint);
                        break;
                    case DHCPMessageType.DHCPRELEASE:
                        if (OnDHCPRelease != null)
                            response = await OnDHCPRelease(request, localEndPoint, remoteEndPoint);
                        break;
                }
                
                if(response != null)
                {
                    var responseBuffer = await response.GetBytes();
                    var sendResult = await listener.SendAsync(responseBuffer, responseBuffer.Length, remoteEndPoint);
                    if (sendResult != responseBuffer.Length)
                        throw new IOException("Failed to transmit DHCP packet");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to process packet : " + e.Message);
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
