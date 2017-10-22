using LibDHCPServer.Enums;
/// The MIT License(MIT)
/// 
/// Copyright(c) 2017 Conscia Norway AS
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.

using LibDHCPServer.Options.VendorSpecificInformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LibDHCPServer.Options
{
    // TODO : Full testing. I implemented this just to make sure the structure is in place.
    public class DHCPOptionVendorSpecificInformation : DHCPOption
    {
        public List<DHCPVendorSpecificInformation> SubOptions { get; set; } = new List<DHCPVendorSpecificInformation>();
        private Dictionary<byte, Type> VSISuboptionTypes = new Dictionary<byte, Type>();

        private byte[] _vendorClassId = new byte[0];
        public byte[] VendorClassId
        {
            get { return _vendorClassId; }
            set
            {
                _vendorClassId = value;

                PopulateVSIObjects();
                ReprocessUnknownSubOptions();
            }
        }

        public DHCPOptionVendorSpecificInformation(byte [] vendorClassId)
        {
            _vendorClassId = vendorClassId.Select(x => x).ToArray();
        }

        public DHCPVendorSpecificInformation ParseSubOption(byte code, byte [] data)
        {
            Type subOptionType;
            if(VSISuboptionTypes.TryGetValue(code, out subOptionType))
            {
                var newSubOption = (DHCPVendorSpecificInformation)Activator
                    .CreateInstance(
                        subOptionType,
                        new object[] {
                            data
                        }
                    );

                return newSubOption;
            }

            return new DHCPVSIUnknown(code, data);
        }

        public DHCPOptionVendorSpecificInformation(int optionLength, byte[] buffer, long offset)
        {
            var index = 0;
            while(index < optionLength)
            {
                var code = buffer[index + offset];
                index++;
                var length = Convert.ToInt32(buffer[index + offset]);
                index++;

                var optionBuffer = new byte[length];
                Array.Copy(buffer, index + offset, optionBuffer, 0, length);
                index += length;

                var subOption = ParseSubOption(code, optionBuffer);
                SubOptions.Add(subOption);
            }
        }

        private void PopulateVSIObjects()
        {
            // TODO : Cache this list as static?
            var vsiObjects =
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(x =>
                        x.Namespace == GetType().Namespace + ".VendorSpecificInformation" &&
                        !x.IsAbstract &&
                        x.BaseType.Name == "DHCPVendorSpecificInformation"
                    )
                    .ToList();

            // TODO : Cache this list as static?
            foreach (var vsiObject in vsiObjects)
            {
                var vsiInstance = (DHCPVendorSpecificInformation)Activator.CreateInstance(vsiObject);
                if (vsiInstance.VendorClassIdentifier.SequenceEqual(_vendorClassId))
                    VSISuboptionTypes[vsiInstance.Code] = vsiObject;
            }
        }

        private void ReprocessUnknownSubOptions()
        {
            var unknownSubOptions = SubOptions
                .Where(x => x.Code == (byte)0)
                .Select(x => x as DHCPVSIUnknown)
                .ToList();

            foreach (var unknownSubOption in unknownSubOptions)
            {
                Type subOptionType;
                if (VSISuboptionTypes.TryGetValue(unknownSubOption.ParsedCode, out subOptionType))
                {
                    var newSubOption = (DHCPVendorSpecificInformation)Activator
                        .CreateInstance(
                            subOptionType,
                            new object[] {
                                unknownSubOption.Data
                            }
                        );

                    SubOptions.Add(newSubOption);
                    SubOptions.Remove(unknownSubOption);
                }
            }
        }

        public override string ToString()
        {
            return "Vendor specific options :" + string.Join(",", SubOptions.Select(x => "{" + x.ToString() + "}").ToList());
        }

        public override async Task Serialize(Stream stream)
        {
            var serializedSubOptions = SubOptions.Select(x => x.Serialize()).ToList();
            var totalLength = serializedSubOptions.Select(x => x.Length).Sum();

            var buffer = new byte[2 + totalLength];
            buffer[0] = Convert.ToByte(DHCPOptionType.VIVendorSpecificInformation);
            buffer[1] = Convert.ToByte(totalLength);

            int index = 2;
            foreach(var subOptionBuffer in serializedSubOptions)
            {
                Array.Copy(subOptionBuffer, 0, buffer, index, subOptionBuffer.Length);
                index += subOptionBuffer.Length;
            }

            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
