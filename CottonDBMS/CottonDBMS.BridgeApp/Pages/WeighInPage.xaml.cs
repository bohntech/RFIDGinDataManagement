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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.BridgeApp.Navigation;

namespace CottonDBMS.BridgeApp.Pages
{
    /// <summary>
    /// Interaction logic for WeighInPage.xaml
    /// </summary>
    public partial class WeighInPage : Page
    {
        public WeighInPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(tbGinTicket);
        }

        private void TbGinTicket_KeyUp(object sender, KeyEventArgs e)
        {
           /* if (tbGinTicket.Text.EndsWith(";"))
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<BarCodeScannedMessage>(new BarCodeScannedMessage { BarCode = tbGinTicket.Text.Trim(';') });
            }*/
        }
    }
}
