using System;
using System.Net.Http;
using System.Threading.Tasks;
using Binance.Spot;
using BinanceApiApp.ViewModels;
using BinanceApiApp.Models;

namespace BinanceApiApp.Services
{
    public class BinanceService
    {
        //private readonly Market market;

        //public BinanceService()
        //{
        //    HttpClient httpClient = new HttpClient();
        //    market = new Market(httpClient);
        //}

        //public async Task<string> GetSymbolPrice(string symbol)
        //{
        //    try
        //    {
        //        var result = await market.SymbolPriceTicker(symbol); // Отримуємо ціну конкретного символу
        //        return $"{result.Price} USDT"; // Правильний доступ до властивості
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Error: {ex.Message}";
        //    }
        //}
    }
}
