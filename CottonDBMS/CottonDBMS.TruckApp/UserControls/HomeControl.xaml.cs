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
using GalaSoft.MvvmLight.Messaging;
using Impinj.OctaneSdk;
using CottonDBMS.TruckApp.DataProviders;
using System.Device.Location;
using CottonDBMS.TruckApp.ViewModels;
using CottonDBMS.TruckApp.Navigation;
using GalaSoft.MvvmLight.Ioc;

namespace CottonDBMS.TruckApp.UserControls
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        HomeViewModel vm = null;
        bool headerIsChecked = false;

        public HomeControl()
        {
            InitializeComponent();          
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (vm == null)
            {
                vm = SimpleIoc.Default.GetInstance<HomeViewModel>();
                DataContext = vm;
                vm.OnFilterChanged += Vm_OnFilterChanged;
            }
            else
            {
                //refresh view model??
            }
        }

        private void Vm_OnFilterChanged(object sender, EventArgs e)
        {
            ((CollectionViewSource)this.Resources["cvsLists"]).View.Refresh();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            PickupListGridItem t = e.Item as PickupListGridItem;
            if (t != null)            
            {
                e.Accepted = (vm.SelectedProducer == null || vm.SelectedProducer.ID == "" || vm.SelectedProducer.DisplayText == t.Client) &&
                             (vm.SelectedFarm == null || vm.SelectedFarm.ID == "" || vm.SelectedFarm.DisplayText == t.Farm) &&
                             (vm.SelectedField == null || vm.SelectedField.ID == "" || vm.SelectedField.DisplayText == t.Field);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            headerIsChecked = !headerIsChecked;
            foreach (var item in dgPickupLists.Items)
            {
                PickupListGridItem t = item as PickupListGridItem;
                t.Checked = headerIsChecked;
            }
        }
    }
}
