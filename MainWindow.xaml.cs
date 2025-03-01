using NetworkAnalyzer.Models;
using NetworkAnalyzer.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NetworkAnalyzer
{
    public partial class MainWindow : Window
    {
        private readonly NetworkService _networkService;
        private readonly UrlService _urlService;
        private readonly PingService _pingService;
        private readonly DnsService _dnsService;
        private readonly HistoryService _historyService;

        public MainWindow()
        {
            InitializeComponent();
            _networkService = new NetworkService();
            _urlService = new UrlService();
            _pingService = new PingService();
            _dnsService = new DnsService();
            _historyService = new HistoryService();

            LoadNetworkInterfaces();
        }

        private void LoadNetworkInterfaces()
        {
            var interfaces = _networkService.GetAllNetworkInterfaces();
            InterfacesListBox.ItemsSource = interfaces;
        }

        private void InterfacesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InterfacesListBox.SelectedItem is NetworkInterfaceInfo info)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Название: " + info.DisplayName);
                sb.AppendLine("Тип: " + info.Type);
                sb.AppendLine("MAC-адрес: " + info.MacAddress);
                sb.AppendLine("Состояние: " + info.OperationalStatus);
                sb.AppendLine("Скорость: " + info.Speed + " бит/с");
                sb.AppendLine("IP-адреса: " + string.Join(", ", info.IpAddresses));
                sb.AppendLine("Маски: " + string.Join(", ", info.SubnetMasks));
                ResultsTextBlock.Text = sb.ToString();
            }
        }

        private void AnalyzeUrl_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Введите корректный URL");
                return;
            }

            try
            {
                UrlInfo info = _urlService.ParseUrl(url);
                _historyService.SaveUrlHistory(url);

                var sb = new StringBuilder();
                sb.AppendLine("Схема: " + info.Scheme);
                sb.AppendLine("Хост: " + info.Host);
                sb.AppendLine("Порт: " + info.Port);
                sb.AppendLine("Путь: " + info.Path);
                sb.AppendLine("Параметры: " + info.Query);
                sb.AppendLine("Фрагмент: " + info.Fragment);
                ResultsTextBlock.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка анализа URL: " + ex.Message);
            }
        }

        private async void Ping_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Введите URL для проверки ping");
                return;
            }

            try
            {
                Uri uri = new Uri(url);
                string host = uri.Host;
                // Вызываем метод, который возвращает строку с итоговой статистикой ping
                string report = await _pingService.PingWithStatisticsAsync(host);
                ResultsTextBlock.Text = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении ping: " + ex.Message);
            }
        }

        private void DnsInfo_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Введите URL для получения DNS информации");
                return;
            }

            try
            {
                Uri uri = new Uri(url);
                string host = uri.Host;
                var dnsInfo = _dnsService.GetDnsInfo(host);
                ResultsTextBlock.Text = dnsInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении DNS информации: " + ex.Message);
            }
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            var history = _historyService.GetUrlHistory();
            ResultsTextBlock.Text = "История проверенных URL:\n" + string.Join("\n", history);
        }
    }
}
