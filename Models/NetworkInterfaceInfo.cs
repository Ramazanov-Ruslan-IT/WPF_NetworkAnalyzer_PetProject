using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NetworkAnalyzer.Models
{
    public class NetworkInterfaceInfo
    {
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string MacAddress { get; set; }
        public OperationalStatus OperationalStatus { get; set; }
        public long Speed { get; set; }
        public List<string> IpAddresses { get; set; }
        public List<string> SubnetMasks { get; set; }
    }
}
