using Binance.Spot;
using BinanceApiApp.Models;
using BinanceApiApp.MVVM;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BinanceApiApp.ViewModels
{
    public class SymbolPriceTickerViewModel: INotifyPropertyChanged
    {
        private readonly Market _market;
        public ObservableCollection<SymbolPrice> Prices { get; } = new();
        public ICommand LoadPricesCommand { get; }

        public SymbolPriceTickerViewModel()
        {
            _market = new Market(new HttpClient());
            LoadPricesCommand = new RelayCommand(async () => await LoadPrices());
            LoadPricesCommand.Execute(null); // Завантажити дані при старті
        }

        private async Task LoadPrices()
        {
            try
            {
                var response = await _market.SymbolPriceTicker();
                var json = JArray.Parse(response);

                Prices.Clear();
                foreach (var item in json)
                {
                    Prices.Add(new SymbolPrice
                    {
                        Symbol = item["symbol"]?.ToString(),
                        Price = item["price"]?.Value<decimal>() ?? 0
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
