using System;
using System.Threading.Tasks;
using Binance.Spot;

class Program
{
    static async Task Main(string[] args)
    {
        Market market = new Market();

        string serverTime = await market.CheckServerTime();

        Console.WriteLine(serverTime);
    }
}
