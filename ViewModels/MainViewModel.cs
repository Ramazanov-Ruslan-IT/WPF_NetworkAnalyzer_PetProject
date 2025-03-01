using NetworkAnalyzer.Models;
using NetworkAnalyzer.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetworkAnalyzer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly NetworkService _networkService;
        private ObservableCollection<NetworkInterfaceInfo> _interfaces;
        public ObservableCollection<NetworkInterfaceInfo> Interfaces
        {
            get => _interfaces;
            set { _interfaces = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _networkService = new NetworkService();
            Interfaces = new ObservableCollection<NetworkInterfaceInfo>(_networkService.GetAllNetworkInterfaces());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
