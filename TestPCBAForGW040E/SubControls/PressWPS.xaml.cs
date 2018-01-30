using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestPCBAForGW040E.Functions;

namespace TestPCBAForGW040E {
    /// <summary>
    /// Interaction logic for PressWPS.xaml
    /// </summary>
    public partial class PressWPS : Window {

        class presswpsinfo : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private string _time;
            private string _title;

            public string time {
                get { return _time; }
                set {
                    _time = value;
                    OnPropertyChanged(nameof(time));
                }
            }

            public string title {
                get { return _title; }
                set {
                    _title = value;
                    OnPropertyChanged(nameof(title));
                }
            }

        }


        public PressWPS(int _title, int _timeout) {
            InitializeComponent();
            int tOut = _timeout / 1000;
            presswpsinfo buttoninfo = new presswpsinfo() { title = _title.ToString(), time = tOut.ToString() };
            this.DataContext = buttoninfo;

            Thread s = new Thread(new ThreadStart(() => {
                int z = 0;
                int tot = tOut;
                while (tot - z > 0) {
                    buttoninfo.time = (tot - z).ToString();
                    Thread.Sleep(1000);
                    z++;
                }
            }));
            s.IsBackground = true;
            s.Start();

            Thread t = new Thread(new ThreadStart(() => {
                while (s.IsAlive) {

                    switch (_title) {
                        case 0: { //WPS
                                for (int i = 0; i < 10; i++) {
                                    string pattern = string.Format("br0: port {0}(ra0) entering disabled state", i);
                                    if (GlobalData.logContent.logviewUART.Contains(pattern)) {
                                        GlobalData.buttonResult = "OK";
                                        GlobalData.logDetailResult.WPSButton = "PASS";
                                        return;
                                    }
                                }
                                break;
                            }
                        case 1: { //Reset
                                if (GlobalData.logContent.logviewUART.Contains("cc.c, 5676 h_sec") || GlobalData.logContent.logviewUART.Contains("cc.c, 5635 h_sec")) {
                                    GlobalData.buttonResult = "OK";
                                    GlobalData.logDetailResult.ResetButton = "PASS";
                                    return;
                                }
                                break;
                            }
                        case 2: { //USB
                                if (GlobalData.logContent.logviewUART.Contains("new SuperSpeed USB device") && GlobalData.logContent.logviewUART.Contains("new high speed USB device")) {
                                    GlobalData.usbResult = "OK";
                                    GlobalData.logDetailResult.USB2 = "PASS";
                                    GlobalData.logDetailResult.USB3 = "PASS";
                                    return;
                                }
                                break;
                            }
                    }
                    Thread.Sleep(100);
                }
                GlobalData.buttonResult = "NG";
                GlobalData.usbResult = "NG";
                GlobalData.logDetailResult.WPSButton = "FAIL";
                GlobalData.logDetailResult.ResetButton = "FAIL";
                GlobalData.logDetailResult.USB2 = "FAIL";
                GlobalData.logDetailResult.USB3 = "FAIL";

            }));
            t.IsBackground = true;
            t.Start();
        }
    }

    
}
