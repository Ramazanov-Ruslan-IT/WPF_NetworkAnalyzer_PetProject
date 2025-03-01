using NetworkAnalyzer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetworkAnalyzer.Services
{
    public class NetworkService
    {
        public List<NetworkInterfaceInfo> GetAllNetworkInterfaces()
        {
            List<NetworkInterfaceInfo> list = new List<NetworkInterfaceInfo>();
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var ipProps = ni.GetIPProperties();
                var info = new NetworkInterfaceInfo
                {
                    DisplayName = ni.Name,
                    Type = ni.NetworkInterfaceType.ToString(),
                    MacAddress = ni.GetPhysicalAddress().ToString(),
                    OperationalStatus = ni.OperationalStatus,
                    Speed = ni.Speed,
                    IpAddresses = ipProps.UnicastAddresses
                                    .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                                    .Select(a => a.Address.ToString()).ToList(),
                    SubnetMasks = ipProps.UnicastAddresses
                                    .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                                    .Select(a => a.IPv4Mask.ToString()).ToList()
                };
                list.Add(info);
            }
            return list;
        }
    }
}
