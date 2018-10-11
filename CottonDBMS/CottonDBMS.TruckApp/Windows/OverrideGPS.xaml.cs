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
using CottonDBMS.TruckApp.DataProviders;

namespace CottonDBMS.TruckApp.Windows
{
    /// <summary>
    /// Interaction logic for OverrideGPS.xaml
    /// </summary>
    public partial class OverrideGPS : Window
    {
        public OverrideGPS()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GPSDataProvider.OverrideLat.HasValue) tbLatitude.Text = GPSDataProvider.OverrideLat.Value.ToString();
            else tbLatitude.Text = "";

            if (GPSDataProvider.OverrideLong.HasValue) tbLongitude.Text = GPSDataProvider.OverrideLong.Value.ToString();
            else tbLongitude.Text = "";
        }

        private void applyGPSOverrideButton_Click(object sender, RoutedEventArgs e)
        {
            double temp;
            bool hasInvalid = false;
            if (!double.TryParse(tbLatitude.Text, out temp))
            {
                MessageBox.Show("Invalid latitude.");
                hasInvalid = true;
            }
            else GPSDataProvider.OverrideLat = temp;

            if (!double.TryParse(tbLongitude.Text, out temp))
            {
                MessageBox.Show("Invalid longitude.");
                hasInvalid = true;
            }
            else GPSDataProvider.OverrideLong = temp;

            if (!hasInvalid) this.Close();
        }

        private void clearOverrideButton_Click(object sender, RoutedEventArgs e)
        {
            GPSDataProvider.OverrideLat = null;
            GPSDataProvider.OverrideLong = null;
            this.Close();
        }

        private void btnYard_Click(object sender, RoutedEventArgs e)
        {
            tbLatitude.Text = "33.688175";
            tbLongitude.Text = "-102.103887";
        }

        private void btnFeeder_Click(object sender, RoutedEventArgs e)
        {
            tbLatitude.Text = "33.675073";
            tbLongitude.Text = "-102.103211";
        }
    }
}
