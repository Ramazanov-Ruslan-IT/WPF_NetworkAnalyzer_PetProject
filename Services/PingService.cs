using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAnalyzer.Services
{
    public class PingService
    {
        public async Task<string> PingWithStatisticsAsync(string host, int count = 4)
        {
            List<PingReply> replies = new List<PingReply>();

            using (Ping pingSender = new Ping())
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        PingReply reply = await pingSender.SendPingAsync(host, 1000);
                        replies.Add(reply);
                    }
                    catch (Exception)
                    {
                        replies.Add(null);
                    }
                }
            }

            int sent = count;
            int received = replies.Count(r => r != null && r.Status == IPStatus.Success);
            int lost = sent - received;
            double lossPercent = (double)lost / sent * 100;

            var successfulReplies = replies
                                    .Where(r => r != null && r.Status == IPStatus.Success)
                                    .Select(r => r.RoundtripTime);

            long minTime = successfulReplies.Any() ? successfulReplies.Min() : 0;
            long maxTime = successfulReplies.Any() ? successfulReplies.Max() : 0;
            double avgTime = successfulReplies.Any() ? successfulReplies.Average() : 0;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Статистика Ping для {host}:");
            sb.AppendLine($"    Пакетов: отправлено = {sent}, получено = {received}, потеряно = {lost}");
            sb.AppendLine($"    ({lossPercent:F0}% потерь)");

            if (received > 0)
            {
                sb.AppendLine("Приблизительное время приема-передачи в мс:");
                sb.AppendLine($"    Минимальное = {minTime} мсек, Максимальное = {maxTime} мсек, Среднее = {avgTime:F1} мсек");
            }

            return sb.ToString();
        }
    }
}
