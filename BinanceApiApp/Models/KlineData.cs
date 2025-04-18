using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceApiApp.Models
{
    public class KlineData
    {
        public DateTime OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }

        public OhlcPoint ToOhlcPoint()
        {
            return new OhlcPoint
            {
                Open = (double)Open,
                High = (double)High,
                Low = (double)Low,
                Close = (double)Close,
            };
        }
    }
}
