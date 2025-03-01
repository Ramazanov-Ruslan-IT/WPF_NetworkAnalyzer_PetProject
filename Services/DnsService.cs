using System.Net;

namespace NetworkAnalyzer.Services
{
    public class DnsService
    {
        public string GetDnsInfo(string host)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(host);
                string ips = string.Join(", ", entry.AddressList.Select(ip => ip.ToString()));
                return $"DNS информация для {host}:\nCanonical Name: {entry.HostName}\nIP адреса: {ips}";
            }
            catch (System.Exception ex)
            {
                return "Ошибка получения DNS информации: " + ex.Message;
            }
        }
    }
}
