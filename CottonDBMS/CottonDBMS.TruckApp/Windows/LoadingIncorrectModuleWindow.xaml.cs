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
using System.Media;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.TruckApp.ViewModels;

namespace CottonDBMS.TruckApp
{
    /// <summary>
    /// Interaction logic for LoadingIncorrectModuleWindow.xaml
    /// </summary>
    public partial class LoadingIncorrectModuleWindow : Window
    {
        private System.Threading.Timer timer = null;

        public LoadingIncorrectModuleWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new System.Threading.Timer(timerCallback, null, 0, 1500);
            //SystemSounds.Beep.Play();
        }

        public void StopBeep()
        {
            timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        private void timerCallback(object state)
        {
            //check network connection            
            //SystemSounds.Beep.Play();
            Console.Beep(2916, 500);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (timer != null)
            {
                LoadingIncorrectModuleViewModel vm = (LoadingIncorrectModuleViewModel) this.DataContext;
                vm.DoCleanup();
                timer.Dispose();              
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IWindowService windowService = SimpleIoc.Default.GetInstance<IWindowService>();
            windowService.FocusLast(WindowType.LoadingIncorrectModuleWindow);
        }
    }
}
