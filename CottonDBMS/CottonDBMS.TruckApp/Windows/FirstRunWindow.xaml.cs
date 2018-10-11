//Licensed under MIT License see LICENSE.TXT in project root folder
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
using System.Security.Permissions;
using CottonDBMS.TruckApp.ViewModels;
using System.IO;
using CottonDBMS.TruckApp.DataProviders;
using CottonDBMS.TruckApp.Messages;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.Interfaces;

namespace CottonDBMS.TruckApp.Windows
{
    /// <summary>
    /// Interaction logic for FirstRunWindow.xaml
    /// </summary>
    public partial class FirstRunWindow : Window
    {
        public FirstRunWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IWindowService windowService = SimpleIoc.Default.GetInstance<IWindowService>();
            windowService.FocusLast(WindowType.WaitingForUnloadWindow);
        }
    }
}
