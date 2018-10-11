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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.ViewModels;

namespace CottonDBMS.TruckApp.UserControls
{
    /// <summary>
    /// Interaction logic for DataSyncSettingsControl.xaml
    /// </summary>
    public partial class DataSyncSettingsControl : UserControl
    {
        DataSyncSettingsViewModel vm = null; 

        public DataSyncSettingsControl()
        {
            InitializeComponent();
        }      
                
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (vm == null)
            {
                vm = SimpleIoc.Default.GetInstance<DataSyncSettingsViewModel>();
                DataContext = vm;
                await vm.InitializeAsync();
            }
        }
    }
}
