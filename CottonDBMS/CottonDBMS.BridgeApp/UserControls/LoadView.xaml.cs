using CottonDBMS.BridgeApp.ViewModels;
using GalaSoft.MvvmLight.Messaging;
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

namespace CottonDBMS.BridgeApp.UserControls
{
    /// <summary>
    /// Interaction logic for LoadView.xaml
    /// </summary>
    public partial class LoadView : UserControl
    {
        public LoadView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSubmitLoad.IsDefault = false;
                LoadViewModel model = (LoadViewModel)this.DataContext;
                model.SerialNumberToAdd = tbSerialNumber.Text;
                model.AddSerialNumberCommand.Execute(this);

                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(250);
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        btnSubmitLoad.IsDefault = true;
                    }));
                });
            }
        }
    }
}
