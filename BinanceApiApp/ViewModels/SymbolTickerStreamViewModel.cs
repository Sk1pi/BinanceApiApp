using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Binance.Spot;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace BinanceApiApp.ViewModels
{
    public class SymbolTickerStreamViewModel: INotifyPropertyChanged, IDisposable
    {
        private MarketDataWebSocket _webSocket;
        private readonly ILogger _logger;
        private CancellationTokenSource _cts;

        public ObservableCollection<SymbolPrice> Prices { get; } = new();
        public string SelectedSymbol { get; set; } = "BTCUSDT";

        public SymbolTickerStreamViewModel()
        {
            // Спрощена ініціалізація логера без AddDebug
            _logger = LoggerFactory.Create(builder => { }).CreateLogger<SymbolTickerStreamViewModel>();

            _cts = new CancellationTokenSource();
            StartWebSocket();
        }

        private void StartWebSocket()
        {
            try
            {
                _webSocket = new MarketDataWebSocket($"{SelectedSymbol.ToLower()}@ticker");

                _webSocket.OnMessageReceived(
                    async (data) =>
                    {
                        try
                        {
                            var json = JObject.Parse(data);

                            await Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                var symbolPrice = new SymbolPrice
                                {
                                    Symbol = json["s"].ToString(),
                                    Price = json["c"].Value<decimal>(),
                                    Volume = json["v"].Value<decimal>(),
                                    PriceChangePercent = json["P"].Value<decimal>()
                                };

                                var existing = Prices.FirstOrDefault(p => p.Symbol == symbolPrice.Symbol);
                                if (existing != null)
                                {
                                    existing.Price = symbolPrice.Price;
                                    existing.Volume = symbolPrice.Volume;
                                    existing.PriceChangePercent = symbolPrice.PriceChangePercent;
                                }
                                else
                                {
                                    Prices.Add(symbolPrice);
                                }

                                OnPropertyChanged(nameof(Prices));
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка обробки повідомлення: {ex.Message}");
                        }
                    },
                    _cts.Token);

                _webSocket.ConnectAsync(_cts.Token);
                Console.WriteLine($"Підключено до WebSocket для {SelectedSymbol}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка ініціалізації WebSocket: {ex.Message}");
            }
        }

        public void ChangeSymbol(string newSymbol)
        {
            SelectedSymbol = newSymbol;
            Dispose();
            StartWebSocket();
        }

        public void Dispose()
        {
            try
            {
                _cts?.Cancel();
                _webSocket?.DisconnectAsync(CancellationToken.None);
                _webSocket?.Dispose();
                Console.WriteLine("WebSocket відключено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при закритті WebSocket: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SymbolPrice: INotifyPropertyChanged
    {
        private decimal _price;
        private decimal _volume;
        private decimal _priceChangePercent;

        public string Symbol { get; set; }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public decimal Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                OnPropertyChanged();
            }
        }

        public decimal PriceChangePercent
        {
            get => _priceChangePercent;
            set
            {
                _priceChangePercent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}