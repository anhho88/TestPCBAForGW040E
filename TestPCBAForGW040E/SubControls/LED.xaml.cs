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

namespace TestPCBAForGW040E
{
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class LED : Window
    {
        class ledState : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private bool _powerled;
            private bool _ponled;
            private bool _inetled;
            private bool _wlanled;
            private bool _lan1led;
            private bool _lan2led;
            private bool _lan3led;
            private bool _lan4led;
            private bool _wpsled;
            private bool _losled;

            private string _time;

            public string time {
                get { return _time; }
                set {
                    _time = value;
                    OnPropertyChanged(nameof(time));
                }
            }

            public bool PowerLED {
                get => _powerled;
                set { _powerled = value; OnPropertyChanged(nameof(PowerLED)); }
            }
            public bool PONLED {
                get => _ponled;
                set { _ponled = value; OnPropertyChanged(nameof(PONLED)); }
            }
            public bool INETLED {
                get => _inetled;
                set { _inetled = value; OnPropertyChanged(nameof(INETLED)); }
            }
            public bool WLANLED {
                get => _wlanled;
                set { _wlanled = value; OnPropertyChanged(nameof(WLANLED)); }
            }
            public bool LAN1LED {
                get => _lan1led;
                set { _lan1led = value; OnPropertyChanged(nameof(LAN1LED)); }
            }
            public bool LAN2LED {
                get => _lan2led;
                set { _lan2led = value; OnPropertyChanged(nameof(LAN2LED)); }
            }
            public bool LAN3LED {
                get => _lan3led;
                set { _lan3led = value; OnPropertyChanged(nameof(LAN3LED)); }
            }
            public bool LAN4LED {
                get => _lan4led;
                set { _lan4led = value; OnPropertyChanged(nameof(LAN4LED)); }
            }
            public bool WPSLED {
                get => _wpsled;
                set { _wpsled = value; OnPropertyChanged(nameof(WPSLED)); }
            }
            public bool LOSLED {
                get => _losled;
                set { _losled = value; OnPropertyChanged(nameof(LOSLED)); }
            }

            public void ON() {
                PowerLED = PONLED = INETLED = WLANLED = LAN1LED = LAN2LED = LAN3LED = LAN4LED = WPSLED = LOSLED = true;
            }

            public void OFF() {
                PowerLED = PONLED = INETLED = WLANLED = LAN1LED = LAN2LED = LAN3LED = LAN4LED = WPSLED = LOSLED = false;
            }
        }

        ledState led = new ledState();

        public LED(int _timeout)
        {
            InitializeComponent();
            int tout = _timeout / 1000;
            led.time = tout.ToString();
            led.ON();
            this.DataContext = led;
            
            Thread s = new Thread(new ThreadStart(() => {
                int z = 0;
                int tot = tout;
                while (tot - z > 0) {
                    led.time = (tot - z).ToString();
                    Thread.Sleep(1000);
                    z++;
                }
            }));
            s.IsBackground = true;
            s.Start();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            CheckBox c = sender as CheckBox;
            if (c.IsChecked==true) c.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF46e81e"));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            CheckBox c = sender as CheckBox;
            if (c.IsChecked == false) c.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD50000"));
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            bool ret = led.PowerLED && led.PONLED && led.INETLED && led.WLANLED && led.LAN1LED && led.LAN2LED && led.LAN3LED && led.LAN4LED && led.WPSLED && led.LOSLED;
            GlobalData.logDetailResult.PowerLED = led.PowerLED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.PONLED = led.PONLED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.INETLED = led.INETLED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.WLANLED = led.WLANLED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.LAN1LED = led.LAN1LED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.LAN2LED = led.LAN2LED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.LAN3LED = led.LAN3LED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.LAN4LED = led.LAN4LED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.WPSLED = led.WPSLED == true ? "PASS" : "FAIL";
            GlobalData.logDetailResult.LOSLED = led.LOSLED == true ? "PASS" : "FAIL";
            if (ret) GlobalData.ledResult = "OK";
            else GlobalData.ledResult = "NG";
        }
    }
}
