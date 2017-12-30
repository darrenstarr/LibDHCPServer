using libnetworkutility;
using System;
using System.Collections;
using System.Net;

namespace LibDHCPServer.VolatilePool
{
    public class DhcpAddressPool
    {
        public DhcpPool Pool { get; }

        private BitArray AvailableAddresses { get; set; }

        public DhcpAddressPool(DhcpPool pool)
        {
            Pool = pool;
            AvailableAddresses = new BitArray(Pool.Network.TotalAddresses);
            AvailableAddresses.SetAll(true);
            AvailableAddresses[0] = false;
            AvailableAddresses[AvailableAddresses.Count - 1] = false;
            foreach(var gateway in Pool.DefaultGateways)
                UnsetAvailable(gateway);
            // TODO : Figure out what the address of the relay would be to remove it from the pool
            foreach (var exclusion in Pool.Exclusions)
                UnsetAvailable(exclusion);
        }

        public void SetAvailable(IPAddress address)
        {
            int index = Convert.ToInt32(address.ToUInt32() - Pool.Network.Network.ToUInt32());
            AvailableAddresses[index] = true;
        }

        public void UnsetAvailable(IPAddress address)
        {
            int index = Convert.ToInt32(address.ToUInt32() - Pool.Network.Network.ToUInt32());
            AvailableAddresses[index] = false;
        }

        public void UnsetAvailable(IPRange range)
        {
            int firstIndex = Convert.ToInt32(range.Start.ToUInt32() - Pool.Network.Network.ToUInt32());
            int lastIndex = Convert.ToInt32(range.End.ToUInt32() - Pool.Network.Network.ToUInt32());
            for(var index=firstIndex; index<=lastIndex; index++)
                AvailableAddresses[index] = false;
        }

        public IPAddress ReserveNextAvailableAddress()
        {
            // TODO : Make our own bit array to allow for optimized searches
            for(var i=0; i<AvailableAddresses.Count; i++)
            {
                if(AvailableAddresses[i])
                {
                    AvailableAddresses[i] = false;
                    return (Pool.Network.Network.ToUInt32() + Convert.ToUInt32(i)).ToIPAddress();
                }
            }
            return null;
        }
    }
}
