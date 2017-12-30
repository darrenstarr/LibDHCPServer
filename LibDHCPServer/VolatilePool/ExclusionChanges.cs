using libnetworkutility;
using System.Collections.Generic;
using System.Linq;

namespace LibDHCPServer.VolatilePool
{
    public class ExclusionChanges
    {
        NetworkPrefix Network { get; set; }
        private List<IPRange> BeforeRanges { get; set; }
        private List<IPRange> AfterRanges { get; set; }

        public ExclusionChanges(NetworkPrefix network, List<IPRange>before, List<IPRange>after)
        {
            Network = network;

            BeforeRanges = before
                .Select(x =>
                    new IPRange
                    {
                        Start = x.Start,
                        End = x.End
                    }
                )
                .ToList();

            AfterRanges = after
                .Select(x =>
                    new IPRange
                    {
                        Start = x.Start,
                        End = x.End
                    }
                )
                .ToList();

        }

        public List<IPRange> ToUnexclude
        {
            get
            {
                return null;
            }
        }
    }
}
