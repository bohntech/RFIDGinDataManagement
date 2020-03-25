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
using CottonDBMS.BridgeApp.ViewModels;

namespace CottonDBMS.BridgeApp.UserControls
{
    /// <summary>
    /// Interaction logic for LoadListControl.xaml
    /// </summary>
    public partial class LoadListControl : UserControl
    {
        public LoadListControl()
        {
            InitializeComponent();
        }

        private void Vm_OnFilterChanged(object sender, EventArgs e)
        {
            ((CollectionViewSource)this.Resources["cvsLoads"]).View.Refresh();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            GinLoadScanViewModel t = e.Item as GinLoadScanViewModel;
            ListViewModel parentVM = (ListViewModel)this.DataContext;
            if (t != null)
            {

                //e.Accepted = (vm.SelectedProducer == null || vm.SelectedProducer.ID == "" || vm.SelectedProducer.DisplayText == t.Client) &&
                //             (vm.SelectedFarm == null || vm.SelectedFarm.ID == "" || vm.SelectedFarm.DisplayText == t.Farm) &&
                //             (vm.SelectedField == null || vm.SelectedField.ID == "" || vm.SelectedField.DisplayText == t.Field);

                DateTime startDate = DateTime.Parse(parentVM.StartDate.ToString("MM/dd/yyyy"));
                DateTime endDate = new DateTime(parentVM.EndDate.Year, parentVM.EndDate.Month, parentVM.EndDate.Day, 23, 59, 59);

                e.Accepted = (t.LastCreatedOrUpdated >= startDate && 
                                t.LastCreatedOrUpdated <= endDate && 
                                (!parentVM.ShowOnlyAuto || (parentVM.ShowOnlyAuto && t.GinTicketLoadNumber.StartsWith("AUTO"))));

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource)this.Resources["cvsLoads"]).View.Refresh();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GinLoadScanViewModel itemVM = (GinLoadScanViewModel)dgItems.SelectedItem;
            ListViewModel parentVM = (ListViewModel)this.DataContext;
            if (parentVM != null && itemVM != null)
            {
                parentVM.OpenCommand.Execute(itemVM);
            }
        }
    }
}
