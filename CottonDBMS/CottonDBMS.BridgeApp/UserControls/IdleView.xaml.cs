using CottonDBMS.BridgeApp.Navigation;
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
using System.Windows.Threading;

namespace CottonDBMS.BridgeApp.UserControls
{
    /// <summary>
    /// Interaction logic for IdleView.xaml
    /// </summary>
    public partial class IdleView : UserControl
    {
            

        public IdleView()
        {
            InitializeComponent();

     
        }

        


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(tbGinTicket);
        }

        private void TextBlock_KeyUp(object sender, KeyEventArgs e)
        {
            /*if (tbGinTicket.Text.EndsWith(";"))
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<BarCodeScannedMessage>(new BarCodeScannedMessage { BarCode = tbGinTicket.Text.Trim(';') });
            }*/
        }
    }
}
