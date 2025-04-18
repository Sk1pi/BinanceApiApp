using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextWpfApiBin.candlestick
{
    /// <summary>
    /// Interaction logic for candlestck.xaml
    /// </summary>
    public partial class candlestck: Window
    {
        public CandlestickViewModel Candlestick { get; }    

        public candlestck()
        {
            InitializeComponent();

            Candlestick = new CandlestickViewModel();

            DataContext = this;
        }
    }
}
