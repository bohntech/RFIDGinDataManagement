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
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.Navigation;


namespace CottonDBMS.TruckApp.Windows
{
    /// <summary>
    /// Interaction logic for UnloadingWindow.xaml
    /// </summary>
    public partial class UnloadingWindow : Window
    {
        public UnloadingWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IWindowService windowService = SimpleIoc.Default.GetInstance<IWindowService>();
            windowService.FocusLast(WindowType.UnloadingAtGin);
        }
    }
}
