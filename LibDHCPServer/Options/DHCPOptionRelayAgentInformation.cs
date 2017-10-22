using LibDHCPServer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    public class DHCPOptionRelayAgentInformation : DHCPOption
    {
        private byte[] _agentCircuitId = new byte[0];
        public byte[] AgentCircuitId
        {
            get { return _agentCircuitId; }
            set
            {
                if (value == null)
                    _agentCircuitId = new byte[0];
                else
                    _agentCircuitId = value;
            }
        }
        private byte[] _agentRemoteId = new byte[0];
        public byte[] AgentRemoteId
        {
            get { return _agentRemoteId; }
            set
            {
                if (value == null)
                    _agentRemoteId = new byte[0];
                else
                    _agentRemoteId = value;
            }
        }

        public DHCPOptionRelayAgentInformation(byte[] agentCircuitId, byte[] agentRemoteId)
        {
            AgentCircuitId = agentCircuitId;
            AgentRemoteId = agentRemoteId;
        }

        public DHCPOptionRelayAgentInformation(string agentCircuitId, string agentRemoteId)
        {
            SetAgentCircuitId(agentCircuitId);
            SetAgentRemoteId(agentRemoteId);
        }

        public void SetAgentCircuitId(string agentCircuitId)
        {
            AgentCircuitId = Encoding.ASCII.GetBytes(agentCircuitId);
        }

        public void SetAgentRemoteId(string agentRemoteId)
        {
            AgentRemoteId = Encoding.ASCII.GetBytes(agentRemoteId);
        }

        public DHCPOptionRelayAgentInformation(int optionLength, byte[] buffer, long offset)
        {
            var index = 0;
            while(index < optionLength)
            {
                var subOption = (DHCPRelayAgentSuboption)buffer[offset + index];
                index++;

                var length = Convert.ToInt32(buffer[offset + index]);
                index++;

                var value = new byte[length];
                Array.Copy(buffer, offset + index, value, 0, length);
                index += length;

                switch(subOption)
                {
                    case DHCPRelayAgentSuboption.AgentCircuitId:
                        AgentCircuitId = value;
                        break;

                    case DHCPRelayAgentSuboption.AgentRemoteId:
                        AgentRemoteId = value;
                        break;

                    default:
                        throw new Exception("Unrecognized DHCP Relay agent information suboption");
                }
            }
        }

        public override string ToString()
        {
            var result = "Relay information = ";
            var count = 0;
            if(AgentCircuitId != null && AgentCircuitId.Length > 0)
            {
                count++;
                if (AgentCircuitId.Select(x => Char.IsControl(Convert.ToChar(x)) ? 1 : 0).Sum() > 0)
                    result += "{Agent Circuit ID = " + string.Join(",", AgentCircuitId.Select(x => string.Format("X2", Convert.ToInt32(x)))) + "}";
                else
                    result += "{Agent Circuit ID = '" + Encoding.ASCII.GetString(AgentCircuitId) + "'}";
            }
            if(AgentRemoteId != null && AgentRemoteId.Length > 0)
            {
                count++;
                if (AgentRemoteId.Select(x => Char.IsControl(Convert.ToChar(x)) ? 1 : 0).Sum() > 0)
                    result += "{Agent Remote ID = " + string.Join(",", AgentRemoteId.Select(x => string.Format("X2", Convert.ToInt32(x)))) + "}";
                else
                    result += "{Agent Remote ID = '" + Encoding.ASCII.GetString(AgentRemoteId) + "'}";
            }
            if(count == 0)
                result += "incomplete";

            return result;
        }

        public override async Task Serialize(Stream stream)
        {
            byte[] circuitIdPart = null;
            if (AgentCircuitId != null && AgentCircuitId.Length > 0)
            {
                circuitIdPart = new byte[2 + AgentCircuitId.Length];
                circuitIdPart[0] = Convert.ToByte(DHCPRelayAgentSuboption.AgentCircuitId);
                circuitIdPart[1] = Convert.ToByte(AgentCircuitId.Length);
                Array.Copy(AgentCircuitId, 0, circuitIdPart, 2, AgentCircuitId.Length);
            }
            else
                circuitIdPart = new byte[0];

            byte[] remoteIdPart = null;
            if (AgentRemoteId != null && AgentRemoteId.Length > 0)
            {
                remoteIdPart = new byte[2 + AgentRemoteId.Length];
                remoteIdPart[0] = Convert.ToByte(DHCPRelayAgentSuboption.AgentRemoteId);
                remoteIdPart[1] = Convert.ToByte(AgentRemoteId.Length);
                Array.Copy(AgentRemoteId, 0, remoteIdPart, 2, AgentRemoteId.Length);
            }
            else
                remoteIdPart = new byte[0];

            if (circuitIdPart.Length == 0 && remoteIdPart.Length == 0)
                throw new Exception("Neither agent circuit ID or agent remote ID are present");

            byte[] buffer = new byte[2];
            buffer[0] = Convert.ToByte(DHCPOptionType.RelayAgentInformation);
            buffer[1] = Convert.ToByte(circuitIdPart.Length + remoteIdPart.Length);
            await stream.WriteAsync(buffer, 0, buffer.Length);
            await stream.WriteAsync(circuitIdPart, 0, circuitIdPart.Length);
            await stream.WriteAsync(remoteIdPart, 0, remoteIdPart.Length);
        }
    }
}
