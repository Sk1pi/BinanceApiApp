using Binance.Spot;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TextWpfApiBin.candlestick
{
    public class CandlestickStreamService: IDisposable
    {
        private MarketDataWebSocket _webSocket;
        private readonly string _symbol;
        private readonly string _interval;
        private readonly Action<string> _onMessageReceived;

        public CandlestickStreamService(string symbol, string interval, Action<string> onMessageReceived)
        {
            _symbol = symbol.ToLower();
            _interval = interval;
            _onMessageReceived = onMessageReceived;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string streamName = $"{_symbol}@kline_{_interval}";
            _webSocket = new MarketDataWebSocket(streamName);

            _webSocket.OnMessageReceived(
                async (data) =>
                {
                    _onMessageReceived(data);
                    await Task.CompletedTask;
                },
                cancellationToken);

            await _webSocket.ConnectAsync(cancellationToken);
        }

        public void Dispose()
        {
            _webSocket?.DisconnectAsync(CancellationToken.None);
            _webSocket?.Dispose();
        }
    }
}
