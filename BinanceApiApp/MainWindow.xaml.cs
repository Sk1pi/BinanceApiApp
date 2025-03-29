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

namespace BinanceApiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainViewModel ViewModel { get; }

        string serverTime = "server time not found";

        public MainWindow()
        {
            InitializeComponent();
            //ViewModel = new MainViewModel();
            //DataContext = ViewModel;
            _ = LoadServerTime();
        }

        private async Task LoadServerTime()
        {
            try
            {
                Market market = new Market();
                string serverTime = await market.CheckServerTime();

                // Оновлюємо UI потік
                Dispatcher.Invoke(() =>
                {
                    ServerTimeText.Text = $"Server Time: {serverTime}";
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    ServerTimeText.Text = $"Помилка: {ex.Message}";
                });
            }
        }
    }
}