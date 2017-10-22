using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionParameterList : DHCPOption
    {
        private List<DHCPOptionType> _parameterList = new List<DHCPOptionType>();
        public List<DHCPOptionType> ParameterList
        {
            get { return _parameterList; }
            set { _parameterList = value.Select(x => x).ToList(); }
        }

        public DHCPOptionParameterList(List<DHCPOptionType> parameterList)
        {
            ParameterList = parameterList;
        }

        public DHCPOptionParameterList(int optionLength, byte[] buffer, long offset)
        {
            for (int i = 0; i < optionLength; i++)
                _parameterList.Add((DHCPOptionType)buffer[offset + i]);
        }

        public override string ToString()
        {
            return "Parameter list - " + ParameterList.Count.ToString() + " entries";
        }

        public override Task Serialize(Stream stream)
        {
            return SerializeBytes(stream, DHCPOptionType.ParameterList, ParameterList.Select(x => Convert.ToByte(x)).ToArray());
        }
    }
}
