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
            btnReload.Visibility = Visibility.Collapsed;
           
            try
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(Properties.Settings.Default.API, Properties.Settings.Default.API)
                });
            }
            catch(Exception ex)
            {
                btnReload.Visibility = Visibility.Visible;
                MessageBox.Show("API ключ не верный или не указан. Введите ключ и нажмите на кнопку ПЕРЕЗАГРУЗИТЬ","Предупреждение");
                logger.Error("API ключ не верный или не указан.");
                return;
            }


            var callResult = await client.FuturesUsdt.Market.GetPricesAsync();

            foreach(var sybmbol in callResult.Data)
            {
                cbPair.Items.Add(sybmbol.Symbol);
            }

            for(int i=1;i<51;i++)
            {
                cbShoulder.Items.Add(i);
            }
            cbShoulder.SelectedValue = cbShoulder.Items[0];
            cbFutures.Items.Add("Лонг");
            cbFutures.Items.Add("Шорт");
            cbFutures.SelectedValue = cbFutures.Items[0];
            LoadDataFromSettings();
            updateCbDelPair();
        }

        private void updateCbDelPair()
        {
            cbDelPair.Items.Clear();
            foreach(DataView symbol in Data)
            {
                cbDelPair.Items.Add(symbol.symbol);
            }
        }

        private void LoadDataFromSettings()
        {
            string []loadStr = Properties.Settings.Default.SelectedPairs.Split(",");

            foreach(string pair in loadStr)
            {
                Data.Add(new DataView { symbol= pair });
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.API = tbAPIkey.Text;
            Properties.Settings.Default.Save();
            LoadInitialData();
        }

        private void btnAddPair_Click(object sender, RoutedEventArgs e)
        {
            string symbol = cbPair.Text;
            foreach (DataView dv in Data)
            {
                if (dv.symbol == symbol)
                {
                    MessageBox.Show("Пара уже добавлена", "Предупреждение");
                    return;
                }
            }

            if (Properties.Settings.Default.SelectedPairs=="")
            {
                Properties.Settings.Default.SelectedPairs = symbol;
            }
            else
            {
                Properties.Settings.Default.SelectedPairs+= ","+symbol;
            }

            Data.Add(new DataView {symbol=symbol });

            cbDelPair.Items.Add(symbol);

            Properties.Settings.Default.Save();
            listJurnal.Items.Add(DateTime.Now+ $" Пара {symbol} добавлена в список") ;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            string delSymbol = cbDelPair.Text;

            //Data.Remove();
            foreach(DataView dv in Data)
            {
                if(dv.symbol==delSymbol)
                {
                    Data.Remove(dv);
                    break;
                }
            }

            List<string> splitSelectedData = Properties.Settings.Default.SelectedPairs.Split(",").ToList();


            splitSelectedData.Remove(delSymbol);

            Properties.Settings.Default.SelectedPairs = String.Join(",", splitSelectedData);
            Properties.Settings.Default.Save();



        }
    }
}
