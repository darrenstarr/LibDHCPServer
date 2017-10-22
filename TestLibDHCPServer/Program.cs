using System;
using LibDHCPServer;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;

namespace TestLibDHCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            //var networkInterfaces = allNetworkInterfaces
            //    .Where(x => 
            //        x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
            //        x.GetIPProperties().UnicastAddresses.Where(y => y.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Count() > 0
            //        );

            //foreach(var networkInterface in networkInterfaces)
            //{
            //    System.Diagnostics.Debug.WriteLine("Interface - " + networkInterface.Name);
            //    System.Diagnostics.Debug.WriteLine("   is type " + networkInterface.NetworkInterfaceType);
            //    var properties = networkInterface.GetIPProperties();
            //    var ipv4Addresses = properties.UnicastAddresses.Where(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList();
            //    foreach (var ipAddress in ipv4Addresses)
            //        System.Diagnostics.Debug.WriteLine("   Address - " + ipAddress.Address.ToString() + "/" + ipAddress.PrefixLength.ToString());
            //    var ipv4Gateways = properties.GatewayAddresses.Where(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList();
            //    foreach (var gateway in ipv4Gateways)
            //        System.Diagnostics.Debug.WriteLine("   Gateway - " + gateway.Address.ToString());
            //}

            Console.WriteLine("Hello World!");
            var server = new Server();
            Task.Factory.StartNew(() => { server.Start(); });
            Thread.Sleep(600000);
        }
    }
}
