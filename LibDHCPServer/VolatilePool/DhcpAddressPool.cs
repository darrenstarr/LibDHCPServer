using libnetworkutility;
using System;
using System.Collections;
using System.Net;

namespace LibDHCPServer.VolatilePool
{
    public class DhcpAddressPool
    {
        public DhcpPool Pool { get; }

        private IPAddressPool AvailableAddresses { get; set; }

        public DhcpAddressPool(DhcpPool pool)
        {
            Pool = pool;
            AvailableAddresses = new IPAddressPool(pool.Network);

            var toUnset = Pool.Exclusions.Clone();
            toUnset.Add(pool.Network.BaseNetwork.Network);
            toUnset.Add(pool.Network.Broadcast);
            toUnset.Add(Pool.DefaultGateways);
            UnsetAvailable(toUnset);
        }

        public void SetAvailable(IPAddress address)
        {
            AvailableAddresses.Unreserve(address);
        }

        public void SetAvailable(IPRange range)
        {
            AvailableAddresses.Unreserve(range);
        }

        public void SetAvailable(IPRanges ranges)
        {
            AvailableAddresses.Unreserve(ranges);
        }

        public void UnsetAvailable(IPAddress address)
        {
            AvailableAddresses.Reserve(address);
        }

        public void UnsetAvailable(IPRange range)
        {
            AvailableAddresses.Reserve(range);
        }

        public void UnsetAvailable(IPRanges ranges)
        {
            AvailableAddresses.Reserve(ranges);
        }

        public IPAddress ReserveNextAvailableAddress()
        {
            return AvailableAddresses.ReserveNextAddress();
        }
    }
}
