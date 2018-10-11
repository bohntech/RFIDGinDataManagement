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
using CottonDBMS.TruckApp.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Impinj.OctaneSdk;
using GalaSoft.MvvmLight.Ioc;

namespace CottonDBMS.TruckApp.UserControls
{
    /// <summary>
    /// Interaction logic for ReaderSettingsControl.xaml
    /// </summary>
    public partial class ReaderSettingsControl : UserControl
    {
        SettingsViewModel vm = null;
        
        public ReaderSettingsControl()
        {
            InitializeComponent();           
        }

        public void Initialize()
        {
            if (vm == null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    vm = SimpleIoc.Default.GetInstance<SettingsViewModel>(); 
                    DataContext = vm;
                    vm.Initialize();
                }));
            }
        }        
    }
}
