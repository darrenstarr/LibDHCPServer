using LibDHCPServer.HardwareAddressTypes;
using libnetworkutility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LibDHCPServer.VolatilePool
{
    public class DHCPPoolManager
    {
        public List<DhcpPool> Pools { get; set; } = new List<DhcpPool>();
        public List<DhcpAddressPool> AddressPools { get; set; } = new List<DhcpAddressPool>();

        public List<Lease> Leases { get; set; } = new List<Lease>();

        public DhcpPool GetPoolByPrefix(NetworkPrefix prefix)
        {
            return Pools.Where(x => x.Network.Equals(prefix)).FirstOrDefault();
        }

        public DhcpPool GetPoolByIP(IPAddress address)
        {
            return Pools
                .Where(x =>
                    x.Network.Contains(address)
                )
                .OrderByDescending(x =>
                    x.Network.Length
                )
                .FirstOrDefault();
        }

        public bool RegisterPool(DhcpPool pool)
        {
            var conflictingPool = GetPoolByPrefix(pool.Network);
            if (conflictingPool != null)
                return false;

            Pools.Add(pool);
            AddressPools.Add(new DhcpAddressPool(pool));

            return true;
        }

        public void DeregisterPool(NetworkPrefix prefix)
        {
            var addressPoolToRemove = AddressPools.Where(x => x.Pool.Network.Equals(prefix)).FirstOrDefault();
            if (addressPoolToRemove == null)
                return;

            AddressPools.Remove(addressPoolToRemove);
            Pools.Remove(addressPoolToRemove.Pool);

            Leases.RemoveAll(x => x.Pool == addressPoolToRemove.Pool);
        }

        public void ModifyPool(
                NetworkPrefix network,
                List<IPAddress> gateways,
                List<IPRange> exclusions,
                string domainName,
                string bootFile,
                List<IPAddress> dnsServers,
                IPAddress tftpServer
            )
        {
            lock (this)
            {
                var addressPool = AddressPools.Where(x => x.Pool.Network.Equals(network)).FirstOrDefault();

                if (addressPool == null)
                    throw new Exception("Address pool for network " + network.ToString() + " could not be found");

                var pool = addressPool.Pool;
                pool.DefaultGateways = gateways
                    .Select(x =>
                        x
                    )
                    .ToList();

                // TODO : update range information in leases and address pool

                var exclusionChanges = new ExclusionChanges(pool.Network, pool.Exclusions, exclusions);

                pool.Exclusions = exclusions
                    .Select(x =>
                        new IPRange
                        {
                            Start = x.Start,
                            End = x.End
                        }
                    )
                    .ToList();

                pool.PoolOptions.DomainName = domainName;
                pool.PoolOptions.BootFile = bootFile;
                pool.PoolOptions.TFTPServers = new List<string> { tftpServer.ToString() };
                pool.PoolOptions.DNSServers = dnsServers
                    .Select(x =>
                        x
                    )
                    .ToList();
            }
        }

        private void CleanPool(DhcpPool pool, DhcpAddressPool addressPool)
        {
            var now = DateTimeOffset.Now;
            var timeoutDeadline = now.Add(pool.RequestTimeOut);

            // TODO : Find better way to clean
            List<Lease> leasesToClean;
            try
            {
                leasesToClean = Leases
                    .Where(x =>
                        x.Pool == pool &&
                        (
                            (
                                x.Acknowledged == null &&
                                x.TimesOut < now
                            ) ||
                            (
                                x.Expires < now
                            )
                        )
                    ).ToList();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to scan leases for leases to clean : " + e.Message);
                return;
            }

            foreach (var lease in leasesToClean)
            {
                try
                {
                    addressPool.SetAvailable(lease.Address);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to set " + lease.Address.ToString() + " as available : " + e.Message);
                    return;
                }

                try
                {
                    Leases.Remove(lease);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to remove " + lease.Address.ToString() + " from leases : " + e.Message);
                    return;
                }
            }
        }
        public Lease ReserveByRelayAddress(IPEndPoint remoteRelay, ClientHardwareAddress clientId, UInt32 transactionId)
        {
            var pool = GetPoolByIP(remoteRelay.Address);
            if (pool == null)
            {
                System.Diagnostics.Debug.WriteLine("No DHCP pool exists for a network containing the IP address : " + remoteRelay.ToString());
                return null;
            }
            System.Diagnostics.Debug.WriteLine("Selected DHCP pool [" + pool.Network.ToString() + "] for request relayed by [" + remoteRelay.ToString() + "]");


            var addressPool = AddressPools.Where(x => x.Pool == pool).FirstOrDefault();
            if (addressPool == null)
                throw new Exception("There is no available address pool for the given DHCP pool");
            System.Diagnostics.Debug.WriteLine("Cleaning DHCP pool - " + pool.Network.ToString());
            CleanPool(pool, addressPool);

            // TODO : Track a counter for pending reservations instead of running large queries
            {
                var leasesFromPool = Leases
                    .Where(x =>
                        x.Pool == pool
                     ).OrderBy(x => 
                        x.Address.ToUInt32()
                     ).ToList();

                var incompleteReservations = leasesFromPool.Where(x => x.Acknowledged == null).Count();
                if (incompleteReservations >= pool.MaxIncompleteRequests)
                {
                    System.Diagnostics.Debug.WriteLine("Warning: There are currently " + incompleteReservations.ToString() + " for the pool " + pool.ToString() + " no reservation will be made. Possible security issue.");
                    return null;
                }
            }

            var nextAvailableAddress = addressPool.ReserveNextAvailableAddress();
            if(nextAvailableAddress == null)
            {
                System.Diagnostics.Debug.WriteLine("No IP addresses were available in pool " + pool.Network.ToString());
                return null;
            }

            var leaseOptions = new LeaseOptions
            {
                BootFile = pool.PoolOptions.BootFile,
                DNSServers = pool.PoolOptions.DNSServers,
                TFTPServers = pool.PoolOptions.TFTPServers,
                Hostname = nextAvailableAddress.ToHexString()
            };

            var now = DateTimeOffset.Now;
            var newLease = new Lease
            {
                Address = nextAvailableAddress,
                Options = leaseOptions,
                Pool = pool,
                Requested = now,
                Expires = now.Add(pool.LeaseDuration),
                TimesOut = now.Add(pool.RequestTimeOut),
                Acknowledged = DateTimeOffset.MinValue,
                Renewed = DateTimeOffset.MinValue,
                ClientId = clientId.Clone(),
                TransactionId = transactionId
            };

            Leases.Add(newLease);

            return newLease;
        }

        public bool ReleaseLease(DHCPPacketView packet, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            var pool = GetPoolByIP(packet.ClientIP);
            if (pool == null)
            {
                System.Diagnostics.Debug.WriteLine("Failed to find DHCP pool for lease release -> " + packet.ClientIP.ToString());
                return false;
            }

            var addressPool = AddressPools.Where(x => x.Pool == pool).FirstOrDefault();
            if (addressPool == null)
            {
                System.Diagnostics.Debug.WriteLine("Failed to to find DHCP IP address pool for lease release -> " + packet.ClientIP.ToString());
                return false;
            }

            addressPool.SetAvailable(packet.ClientIP);
            System.Diagnostics.Debug.WriteLine("Returned address " + packet.ClientIP.ToString() + " to the address pool");

            var count = Leases
                .RemoveAll(x =>
                    x.ClientId.Equals(packet.ClientId)
                );

            if(count == 0)
            {
                System.Diagnostics.Debug.WriteLine("DHCPPools did not have a lease to be released for client id " + packet.ClientId.ToString());
                return false;
            }

            System.Diagnostics.Debug.WriteLine("DHCPPools released IP address " + packet.ClientIP.ToString() + " for client id " + packet.ClientId.ToString());

            return true;
        }
    }
}
