using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using DevExpress.Xpo.Logger;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PurchasesSaleBinance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ObservableCollection<DataView> Data = new ObservableCollection<DataView>();
        BinanceClient client = new BinanceClient();
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logger.Info("Старт приложения");

            Table.ItemsSource = Data;
            LoadInitialData();

            tbAPIkey.Text = Properties.Settings.Default.API;
        }

        private async void LoadInitialData()
        {
           
            try
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(Properties.Settings.Default.API, Properties.Settings.Default.API)
                });
            }
            catch(Exception ex)
            {
                
                MessageBox.Show("API ключ не верный или не указан. Введите ключ и нажмите на кнопку ПЕРЕЗАГРУЗИТЬ","Предупреждение");
                logger.Error("API ключ не верный или не указан.");
                return;
            }


            var callResult = await client.FuturesUsdt.Market.GetPricesAsync();

        }
    }
}
