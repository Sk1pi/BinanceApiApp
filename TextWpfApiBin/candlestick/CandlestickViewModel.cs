using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TextWpfApiBin.Base;

namespace TextWpfApiBin.candlestick
{
    public class CandlestickViewModel: BaseViewModel
    {
        private CancellationTokenSource _cts;
        private CandlestickStreamService _streamService;

        public SeriesCollection SeriesCollection { get; }
        public List<DateTime> TimeValues { get; } = new List<DateTime>();

        private string _symbol = "BTCUSDT";
        public string Symbol
        {
            get => _symbol;
            set
            {
                _symbol = value;
                RestartStream();
            }
        }

        private string _interval = "5m";
        public string Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                RestartStream();
            }
        }

        private bool IsValidInterval(string interval)
        {
            string[] validIntervals = { "1m", "5m", "15m", "1h", "4h", "1d" };
            return Array.Exists(validIntervals, x => x == interval);
        }

        public CandlestickViewModel()
        {
            SeriesCollection = new SeriesCollection
        {
            new CandleSeries
            {
                Values = new ChartValues<OhlcPoint>(),
                StrokeThickness = 1
            }
        };

            StartStream();
        }

        public void StartStream()
        {
            _cts = new CancellationTokenSource();
            _streamService = new CandlestickStreamService(Symbol, Interval, HandleWebSocketMessage);
            Task.Run(() => _streamService.StartAsync(_cts.Token));
        }

        private void HandleWebSocketMessage(string jsonData)
        {
            try
            {
                var json = JObject.Parse(jsonData);
                var kline = json["k"];

                var candle = new OhlcPoint
                {
                    Open = kline["o"].Value<double>(),
                    High = kline["h"].Value<double>(),
                    Low = kline["l"].Value<double>(),
                    Close = kline["c"].Value<double>()
                };

                var openTime = DateTimeOffset.FromUnixTimeMilliseconds(kline["t"].Value<long>()).DateTime;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SeriesCollection[0].Values.Add(candle);
                    TimeValues.Add(openTime);

                    // Обмежуємо кількість відображуваних свічок
                    if (SeriesCollection[0].Values.Count > 100)
                    {
                        SeriesCollection[0].Values.RemoveAt(0);
                        TimeValues.RemoveAt(0);
                    }

                    OnPropertyChanged(nameof(SeriesCollection));
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        public void RestartStream()
        {
            _cts?.Cancel();
            _streamService?.Dispose();
            StartStream();
        }

        public override void Dispose()
        {
            _cts?.Cancel();
            _streamService?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
