using Binance.Spot;
using Binance.Spot.Models;
using BinanceApiApp.Models;
using BinanceApiApp.ViewModels.Base;
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
    public class MainViewModel: BaseViewModel
    {
        public CandlestickViewModel Candlestick { get; }
        public SymbolPriceTickerViewModel PriceTicker { get; }
        public SymbolTickerStreamViewModel _liveTicker;
        public SymbolTickerStreamViewModel LiveTicker
        {
            get => _liveTicker;
            set
            {
                _liveTicker = value;
                OnPropertyChanged(); // Сповіщення для WPF
            }
        }

        public SymbolTickerStreamViewModel SymbolTicker { get; } = new();
        public ICommand RefreshCommand { get; }

        public MainViewModel()
        {
            Candlestick = new CandlestickViewModel();
            LiveTicker = new SymbolTickerStreamViewModel();
            PriceTicker = new SymbolPriceTickerViewModel();

            //RefreshCommand = new RelayCommand(() =>
            //{
            //    PriceTicker.LoadPricesCommand.Execute(null);
            //    Candlestick.RestartStream();
            //});
        }
    }
}
