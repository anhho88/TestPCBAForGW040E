using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PlugLAN.xaml
    /// </summary>
    public partial class PlugLAN : Window {

        int InOut = 0, timeOut = 0;
        LANData lan = new LANData();

        public PlugLAN(int _inOut, int _timeout) {
            InitializeComponent();
            this.InOut = _inOut;
            this.timeOut = _timeout;
            this.DataContext = lan;
            lblTitle.Content = this.InOut == 0 ? "Plug in LAN cables" : "Plug out LAN cables";

            Thread s = new Thread(new ThreadStart(() => {
                int z = 0;
                int tot = timeOut / 1000;
                while (tot - z > 0) {
                    lan.time = (tot - z).ToString();
                    Thread.Sleep(1000);
                    z++;
                }
            }));
            s.IsBackground = true;
            s.Start();

            Thread t = new Thread(new ThreadStart(() => {
                while (s.IsAlive) {

                    switch (InOut) {
                        case 0: { //Plugin LAN
                                lan.Off();
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_1 up")) lan.INOUT1 = true;
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_2 up")) lan.INOUT2 = true;
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_3 up")) lan.INOUT3 = true;
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_4 up")) lan.INOUT4 = true;

                                if (lan.INOUT1==true && lan.INOUT2==true && lan.INOUT3==true && lan.INOUT4==true) {
                                    GlobalData.lanResult = "OK";
                                    return;
                                }
                                break;
                            }
                        case 1: { //Plugout LAN
                                lan.On();
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_1 down")) lan.INOUT1 = false;
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_2 down")) lan.INOUT2 = false;
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_3 down")) lan.INOUT3 = false;
                                if (GlobalData.logContent.logviewUART.Contains("Link State: LAN_4 down")) lan.INOUT4 = false;

                                if (lan.INOUT1 == false && lan.INOUT2 == false && lan.INOUT3 == false && lan.INOUT4 == false) {
                                    GlobalData.lanResult = "OK";
                                    return;
                                }
                                break;
                            }
                    }
                    Thread.Sleep(100);
                }
                GlobalData.lanResult = "NG";
            }));
            t.IsBackground = true;
            t.Start();
        }
    }

    public class LANData : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _time;
        private bool _inout1;
        private bool _inout2;
        private bool _inout3;
        private bool _inout4;

        public string time {
            get { return _time; }
            set {
                _time = value;
                OnPropertyChanged(nameof(time));
            }
        }

        public bool INOUT1 {
            get { return _inout1; }
            set {
                _inout1 = value;
                OnPropertyChanged(nameof(INOUT1));
            }
        }
        public bool INOUT2 {
            get { return _inout2; }
            set {
                _inout2 = value;
                OnPropertyChanged(nameof(INOUT2));
            }
        }
        public bool INOUT3 {
            get { return _inout3; }
            set {
                _inout3 = value;
                OnPropertyChanged(nameof(INOUT3));
            }
        }
        public bool INOUT4 {
            get { return _inout4; }
            set {
                _inout4 = value;
                OnPropertyChanged(nameof(INOUT4));
            }
        }

        public void Off() {
            this.INOUT1 = false;
            this.INOUT2 = false;
            this.INOUT3 = false;
            this.INOUT4 = false;
        }

        public void On() {
            this.INOUT1 = true;
            this.INOUT2 = true;
            this.INOUT3 = true;
            this.INOUT4 = true;
        }

    }

}
