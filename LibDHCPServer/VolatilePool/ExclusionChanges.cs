using libnetworkutility;
using System.Collections.Generic;
using System.Linq;

namespace LibDHCPServer.VolatilePool
{
    public class ExclusionChanges
    {
        NetworkPrefix Network { get; set; }
        private IPRanges BeforeRanges { get; set; }
        private IPRanges AfterRanges { get; set; }

        public ExclusionChanges(NetworkPrefix network, IPRanges before, IPRanges after)
        {
            Network = network;

            BeforeRanges = before.Clone();
            AfterRanges = after.Clone();
        }

        public IPRanges ToUnexclude
        {
            get
            {
                return BeforeRanges.Minus(AfterRanges);
            }
        }

        public IPRanges ToExclude
        {
            get
            {
                return AfterRanges.Minus(BeforeRanges);
            }
        }
    }
}
