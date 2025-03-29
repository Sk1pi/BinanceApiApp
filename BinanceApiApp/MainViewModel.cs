using Binance.Spot;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BinanceApiApp
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private string _serverTime = "serverTime not found";
        public string ServerTime
        {
            get => _serverTime;
            set
            {
                if (_serverTime != value)
                {
                    _serverTime = value;
                    OnPropertyChanged(nameof(ServerTime));
                }
            }
        }

        public async Task CheckServerTime()
        {
            Market market = new Market();
            string time = await market.CheckServerTime();

            App.Current.Dispatcher.Invoke(() =>
            {
                ServerTime = time;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
