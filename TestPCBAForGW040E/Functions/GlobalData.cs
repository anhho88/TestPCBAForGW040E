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
    }
}
