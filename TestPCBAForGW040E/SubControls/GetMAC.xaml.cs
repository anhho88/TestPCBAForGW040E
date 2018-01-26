using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TestPCBAForGW040E.Functions;

namespace TestPCBAForGW040E {
    /// <summary>
    /// Interaction logic for GetMAC.xaml
    /// </summary>
    public partial class GetMAC : Window {

        Countdown t = new Countdown();
        int retry = 0;
        int index = 0;

        public GetMAC(int ret, int timeout) {
            InitializeComponent();
            this.DataContext = t;
            retry = ret;

            Thread s = new Thread(new ThreadStart(() => {
                int z = 0;
                int tot = timeout / 1000;
                while (tot - z > 0) {
                    t.time = (tot - z).ToString();
                    Thread.Sleep(1000);
                    z++;
                }
            }));
            s.IsBackground = true;
            s.Start();
            txtMacAddress.Focus();
        }

        private void txtMacAddress_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                //Check time out
                if (index + 1 >= retry) {
                    GlobalData.macAddress = "ERROR";
                    return;
                }

                //get settings mac address
                List<string> listMac = new List<string>();
                if (!GlobalData.defaultSettings.DUT_MACFormat.Contains(":")) {
                    listMac.Add(GlobalData.defaultSettings.DUT_MACFormat);
                }
                else {
                    string[] buffer = GlobalData.defaultSettings.DUT_MACFormat.Split(':');
                    for (int i = 0; i < buffer.Length; i++) {
                        listMac.Add(buffer[i]);
                    }
                }
                //format mac address
                string mac = txtMacAddress.Text.ToUpper().Trim();
                mac = mac.Replace(":", "");
                //check mac length
                if (mac.Length != 12) goto NG;
                //check first 6 digits of MAC
                bool isTrue = false;
                string s = mac.Substring(0, 6);
                foreach (var item in listMac) {
                    if (s == item) {
                        isTrue = true;
                        break;
                    }
                }
                if (!isTrue) goto NG;
                //check end 6 digits of MAC
                string et = mac.Replace(s, "");
                bool ret = System.Text.RegularExpressions.Regex.IsMatch(et, "[0-9,A-F][0-9,A-F][0-9,A-F][0-9,A-F][0-9,A-F][0-9,A-F]");
                if (!ret) goto NG;
                else goto OK;

                NG:
                {
                    txtMacAddress.Clear();
                    index++;
                    return;
                }
                OK:
                {
                    GlobalData.macAddress = mac;
                    return;
                }
            }
        }
    }

}
