using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace TestPCBAForGW040E.Functions
{
    public static class GlobalData
    {
        public static ObservableCollection<ContentGridFields> uploadFWContent = new ObservableCollection<ContentGridFields>();
        public static ObservableCollection<ContentGridFields> writeMacContent = new ObservableCollection<ContentGridFields>();
        public static ObservableCollection<ContentGridFields> checkLanContent = new ObservableCollection<ContentGridFields>();
        public static ObservableCollection<ContentGridFields> checkUsbContent = new ObservableCollection<ContentGridFields>();
        public static ObservableCollection<ContentGridFields> checkButtonContent = new ObservableCollection<ContentGridFields>();
        public static ObservableCollection<ContentGridFields> checkLedContent = new ObservableCollection<ContentGridFields>();

        public static PropertiesDefaultSetting defaultSettings = new PropertiesDefaultSetting();
        public static TestingUpdateStatus TestingContent = new TestingUpdateStatus();

        public static LoadTestCaseContent loadtestcasecontent = new LoadTestCaseContent();
        public static MainWindowInfor mainwindowInformation = new MainWindowInfor();

        public static logViewContent logContent = new logViewContent();
        public static string uartData = "";
        public static RS232 serialPort = new RS232();

        public static string macAddress = "";
        public static string lanResult = "";
        public static string buttonResult = "";
        public static string usbResult = "";
        public static string ledResult = "";

        public static logInfomation logResult;
        public static logDetailInfo logDetailResult;
    }

    public static class StringExtensions {

        public static bool isUsb3ConnectSuccess(this string s) {
            return Regex.IsMatch(s, ".\\d+-\\d+.\\d+: new SuperSpeed USB device+");
        }

        public static bool isUsb2ConnectSuccess(this string s) {
            return Regex.IsMatch(s, ".\\d+-\\d+.\\d+: new high speed USB device+");
        }

        public static bool isWifiBootCompleted(this string s) {
            return s.Contains("br0: port 1(ra0) entering forwarding state");
        }

        public static bool isWPSPressed(this string s) {
            return s.Contains("br0: port 1(ra0) entering disabled state");
        }
    }
}
