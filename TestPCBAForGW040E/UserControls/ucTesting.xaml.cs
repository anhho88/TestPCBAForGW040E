using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPCBAForGW040E.Functions;

namespace TestPCBAForGW040E.UserControls {
    /// <summary>
    /// Interaction logic for ucTesting.xaml
    /// </summary>
    public partial class ucTesting : UserControl {

        List<Control> checkboxControls = null;

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            checkboxControls = new List<Control>() { ckFW, ckMAC, ckLAN, ckUSB, ckBUTTON, ckLED };
            this.DataContext = GlobalData.TestingContent;
            GlobalData.loadtestcasecontent.load();
            this.fwDataGrid.ItemsSource = GlobalData.uploadFWContent;
            this.macDataGrid.ItemsSource = GlobalData.writeMacContent;
            this.lanDataGrid.ItemsSource = GlobalData.checkLanContent;
            this.usbDataGrid.ItemsSource = GlobalData.checkUsbContent;
            this.buttonDataGrid.ItemsSource = GlobalData.checkButtonContent;
            this.ledDataGrid.ItemsSource = GlobalData.checkLedContent;
        }

        public ucTesting() {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch(b.Content) {
                case "Configure": {
                        b.Content = "Save";
                        foreach (CheckBox c in checkboxControls) {
                            c.IsEnabled = true;
                        }
                        break;
                    }
                case "Save": {
                        b.Content = "Configure";
                        foreach (CheckBox c in checkboxControls) {
                            c.IsEnabled = false;
                        }
                        GlobalData.defaultSettings.Save();
                        break;
                    }
                case "Default": {
                        foreach (CheckBox c in checkboxControls) {
                            c.IsChecked = true;
                        }
                        break;
                    }
            }
        }

        private void DataGrid_LostFocus(object sender, RoutedEventArgs e) {
            DataGrid datagrid = sender as DataGrid;
            datagrid.UnselectAllCells();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            GlobalData.loadtestcasecontent.reload();
            this.fwDataGrid.Items.Refresh();
            ///////////////////////////////////////////////////////////////////////////
            Thread t = new Thread(new ThreadStart(() => {
                //var settings = Properties.Settings.Default;
                ////Upload FW
                //string errorMessage;
                //if (settings.flag_FW=="1") {
                //    if (!(new exUploadFW().uploadFWtoDUT(out errorMessage))) return;
                //}
            }));
            t.IsBackground = true;
            t.Start();

            ///////////////////////////////////////////////////////////////////////////
            Thread r = new Thread(new ThreadStart(() => {
                bool _flag = false;
                int counter = 0;
                while (!_flag) {
                    try {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                            this.fwDataGrid.Items.Refresh();
                        }));
                    }
                    catch { }
                    Thread.Sleep(500);
                    if (!t.IsAlive) counter++;
                    if (counter >= 3) _flag = true;
                }
            }));
            r.IsBackground = true;
            r.Start();
            ///////////////////////////////////////////////////////////////////////////
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            GlobalData.uploadFWContent[0].JUDGED = "PASS";
            this.fwDataGrid.Items.Refresh();
        }
    }
}
