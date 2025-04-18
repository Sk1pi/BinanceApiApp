using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Binance.Spot;
using Binance.Spot.Models;
using BinanceApiApp.ViewModels;

namespace BinanceApiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CandlestickViewModel Candlestick { get; }
        public SymbolTickerStreamViewModel SymbolTicker { get; }
        public MainViewModel MainViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            //MainViewModel = new MainViewModel();
            //Candlestick = new CandlestickViewModel();
            //SymbolTicker = new SymbolTickerStreamViewModel();
            DataContext = new MainViewModel();
            //DataContext = new CandlestickViewModel()
            //DataContext = new SymbolTickerStreamViewModel();

            Loaded += (s, e) =>
            {
                var viewModel = (MainViewModel)DataContext;
                viewModel.SymbolTicker.Prices.Add(new SymbolPrice { Symbol = "TEST", Price = 100 });
            };

            //DataContext = this;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}