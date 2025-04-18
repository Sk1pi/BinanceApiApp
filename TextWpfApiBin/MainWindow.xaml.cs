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
using TextWpfApiBin.candlestick;
using TextWpfApiBin.SymbolTickerStream;

namespace TextWpfApiBin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public SymbolTickerStreamViewModel SymbolTickerStream { get; }
        public CandlestickViewModel Candlestick { get; }
        public MainWindow()
        {
            InitializeComponent();
            //SymbolTickerStream = new SymbolTickerStreamViewModel();
            //Candlestick = new CandlestickViewModel();

            DataContext = new SymbolTickerStreamViewModel();

            //DataContext = this;
        }
    }
}