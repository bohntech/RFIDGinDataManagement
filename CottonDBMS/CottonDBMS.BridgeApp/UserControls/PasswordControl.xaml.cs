using CottonDBMS.BridgeApp.ViewModels;
using CottonDBMS.Bridges.Shared.ViewModels;
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
    /// Interaction logic for PasswordControl.xaml
    /// </summary>
    public partial class PasswordControl : UserControl
    {
        public PasswordControl()
        {
            InitializeComponent();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            SettingsPageViewModel vm = (SettingsPageViewModel)this.DataContext;
            vm.Password = tbPassword.Password;
        }

        private void TbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SettingsPageViewModel vm = (SettingsPageViewModel)this.DataContext;
            vm.Password = tbPassword.Password;
        }
    }
}
