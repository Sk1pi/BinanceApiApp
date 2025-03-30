using Binance.Spot;
using Binance.Spot.Models;
using BinanceApiApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BinanceApiApp.MVVM;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BinanceApiApp.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private readonly Market _market;
        private readonly HttpClient _httpClient;
        public ObservableCollection<KlineData> Klines { get; } = new();
        public ICommand LoadDataCommand { get; }

        public MainViewModel()
        {
            _httpClient = new HttpClient();
            _market = new Market(_httpClient);
            LoadDataCommand = new RelayCommand(async () => await LoadKlineData());
            LoadDataCommand.Execute(null);
        }

        private async Task LoadKlineData()
        {
            try
            {
                Klines.Clear();
                var response = await _market.KlineCandlestickData("BNBUSDT", Interval.ONE_MINUTE);
                var json = JArray.Parse(response);

                foreach (var kline in json)
                {
                    Klines.Add(new KlineData
                    {
                        OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(kline[0].Value<long>()).DateTime,
                        Open = kline[1].Value<decimal>(),
                        High = kline[2].Value<decimal>(),
                        Low = kline[3].Value<decimal>(),
                        Close = kline[4].Value<decimal>(),
                        Volume = kline[5].Value<decimal>(),
                        Price = kline[4].Value<decimal>()
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
