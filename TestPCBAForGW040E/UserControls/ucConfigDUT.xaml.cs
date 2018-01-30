using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TestPCBAForGW040E.Functions;
using Microsoft.Win32;

namespace TestPCBAForGW040E.UserControls
{
    /// <summary>
    /// Interaction logic for ucSettingDUT.xaml
    /// </summary>
    public partial class ucConfigDUT : UserControl
    {

        List<string> baudRates = new List<string>() {
            "50","75","110","134","150","200","300","600",
            "1200","1800","2400","4800","9600",
            "19200","28800","38400","57600","76800",
            "115200","230400","460800","576000","921600"
        };
        List<string> portNames = new List<string>();

        public ucConfigDUT()
        {
            InitializeComponent();
            this.DataContext = GlobalData.defaultSettings;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch (b.Content) {
                case "Save": {
                        GlobalData.defaultSettings.DUT_SerialPortName = cbbPortName.Text;
                        GlobalData.defaultSettings.DUT_SerialBaudRate = cbbBaudRate.Text;
                        GlobalData.defaultSettings.Save();
                        MessageBox.Show("Success!", "Save DUT config", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                case "Default": {
                        break;
                    }
                case "browser": {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "firmware *.bin|*.bin";
                        openFileDialog.Title = "Select path of file 'tclinux.bin'";
                        openFileDialog.FileName = "tclinux.bin";
                        if (openFileDialog.ShowDialog()== true) {
                            GlobalData.defaultSettings.FWPath = openFileDialog.FileName;
                        }
                        break;
                    }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            for (int i = 1; i < 100; i++) {
                portNames.Add(string.Format("COM{0}", i));
            }
            cbbPortName.ItemsSource = portNames;
            cbbPortName.Text = GlobalData.defaultSettings.DUT_SerialPortName;
            cbbBaudRate.ItemsSource = this.baudRates;
            cbbBaudRate.Text = GlobalData.defaultSettings.DUT_SerialBaudRate;
        }
    }
}
