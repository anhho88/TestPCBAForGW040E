using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Reflection;

namespace TestPCBAForGW040E.Functions {

    public class ContentGridFields {

        public ContentGridFields() { }

        public ContentGridFields(params string[] datas) {
            this.INDEX = datas[0];
            this.STEPNAME = datas[1];
            this.STANDARD = datas[2];
            this.ACTUAL = datas[3];
            this.RETRY = datas[4];
            this.TIMEOUT = datas[5];
            this.JUDGED = datas[6];
        }

        public string INDEX { get; set; }
        public string STEPNAME { get; set; }
        public string STANDARD { get; set; }
        public string ACTUAL { get; set; }
        public string RETRY { get; set; }
        public string TIMEOUT { get; set; }
        public string JUDGED { get; set; }
    }

    public class MainWindowInfor : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _isPortOpenSuccess = false;
        private string _statustestContent;
        private bool _uartnewdatasign = false;
        private bool _wpsnewdatasign = false;
        private bool _sysnewdatasign = false;
        private bool _istesting = false;

        public bool isPortOpenSuccess {
            get { return _isPortOpenSuccess; }
            set {
                _isPortOpenSuccess = value;
                OnPropertyChanged(nameof(isPortOpenSuccess));
            }
        }
        public string statustestContent {
            get { return _statustestContent; }
            set {
                _statustestContent = value;
                OnPropertyChanged(nameof(statustestContent));
            }
        }
        public bool UARTDataSign {
            get { return _uartnewdatasign; }
            set {
                _uartnewdatasign = value;
                OnPropertyChanged(nameof(UARTDataSign));
            }
        }
        public bool WPSDataSign {
            get { return _wpsnewdatasign; }
            set {
                _wpsnewdatasign = value;
                OnPropertyChanged(nameof(WPSDataSign));
            }
        }
        public bool SYSDataSign {
            get { return _sysnewdatasign; }
            set {
                _sysnewdatasign = value;
                OnPropertyChanged(nameof(SYSDataSign));
            }
        }
        public bool IsTesting {
            get { return _istesting; }
            set {
                _istesting = value;
                OnPropertyChanged(nameof(IsTesting));
            }
        }
    }

    public class logViewContent : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _logviewUART;
        private string _logviewWPS;
        private string _logviewSystem;

        public string logviewUART {
            get { return _logviewUART; }
            set {
                _logviewUART = value;
                OnPropertyChanged(nameof(logviewUART));
            }
        }

        public string logviewWPS {
            get { return _logviewWPS; }
            set {
                _logviewWPS = value;
                OnPropertyChanged(nameof(logviewWPS));
            }
        }

        public string logviewSystem {
            get { return _logviewSystem; }
            set {
                _logviewSystem = value;
                OnPropertyChanged(nameof(logviewSystem));
            }
        }
    }

    public class LoadTestCaseContent {

        enum localfiles { FW, MAC, LAN, USB, BUTTON, LED };

        private ObservableCollection<ContentGridFields> _loadfilecontent(localfiles _file) {
            ObservableCollection<ContentGridFields> t = new ObservableCollection<ContentGridFields>();
            try {
                string SettingPath = string.Format("{0}\\Settings", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                string[] lines = System.IO.File.ReadAllLines(string.Format("{0}\\{1}.csv", SettingPath, _file.ToString()));
                foreach (var line in lines) {
                    if (line.Trim().Replace("\n", "").Replace("\r", "") != null && line.Contains("INDEX,STEPNAME,STANDARD,ACTUAL,RETRY,TIMEOUT,JUDGED") != true) {
                        string[] buffer = line.Split(',');
                        ContentGridFields c = new ContentGridFields(buffer);
                        t.Add(c);
                    }
                }
            } catch (Exception ex) {
                GlobalData.logContent.logviewSystem += ex.ToString();
            }
            return t.Count > 0 ? t : null;
        }

        private ObservableCollection<ContentGridFields> loadFWContent() {
            return _loadfilecontent(localfiles.FW);
        }
        private ObservableCollection<ContentGridFields> loadMACContent() {
            return _loadfilecontent(localfiles.MAC);
        }
        private ObservableCollection<ContentGridFields> loadLANContent() {
            return _loadfilecontent(localfiles.LAN);
        }
        private ObservableCollection<ContentGridFields> loadUSBContent() {
            return _loadfilecontent(localfiles.USB);
        }
        private ObservableCollection<ContentGridFields> loadBUTTONContent() {
            return _loadfilecontent(localfiles.BUTTON);
        }
        private ObservableCollection<ContentGridFields> loadLEDContent() {
            return _loadfilecontent(localfiles.LED);
        }

        public void load() {

            GlobalData.uploadFWContent = null;
            GlobalData.writeMacContent = null;
            GlobalData.checkLanContent = null;
            GlobalData.checkUsbContent = null;
            GlobalData.checkButtonContent = null;
            GlobalData.checkLedContent = null;

            GlobalData.uploadFWContent = this.loadFWContent();
            GlobalData.writeMacContent = this.loadMACContent();
            GlobalData.checkLanContent = this.loadLANContent();
            GlobalData.checkUsbContent = this.loadUSBContent();
            GlobalData.checkButtonContent = this.loadBUTTONContent();
            GlobalData.checkLedContent = this.loadLEDContent();
        }

        public void reload() {
            for(int i = 0; i < GlobalData.uploadFWContent.Count; i++) {
                GlobalData.uploadFWContent[i].ACTUAL = "?";
                GlobalData.uploadFWContent[i].JUDGED = "?";
            }
            for (int i = 0; i < GlobalData.writeMacContent.Count; i++) {
                GlobalData.writeMacContent[i].ACTUAL = "?";
                GlobalData.writeMacContent[i].JUDGED = "?";
            }
            for (int i = 0; i < GlobalData.checkLanContent.Count; i++) {
                GlobalData.checkLanContent[i].ACTUAL = "?";
                GlobalData.checkLanContent[i].JUDGED = "?";
            }
            for (int i = 0; i < GlobalData.checkUsbContent.Count; i++) {
                GlobalData.checkUsbContent[i].ACTUAL = "?";
                GlobalData.checkUsbContent[i].JUDGED = "?";
            }
            for (int i = 0; i < GlobalData.checkButtonContent.Count; i++) {
                GlobalData.checkButtonContent[i].ACTUAL = "?";
                GlobalData.checkButtonContent[i].JUDGED = "?";
            }
            for (int i = 0; i < GlobalData.checkLedContent.Count; i++) {
                GlobalData.checkLedContent[i].ACTUAL = "?";
                GlobalData.checkLedContent[i].JUDGED = "?";
            }

            GlobalData.TestingContent.fwStatusContent = "";
            GlobalData.TestingContent.macStatusContent = "";
            GlobalData.TestingContent.lanStatusContent = "";
            GlobalData.TestingContent.usbStatusContent = "";
            GlobalData.TestingContent.buttonStatusContent = "";
            GlobalData.TestingContent.ledStatusContent = "";
        }

    }

    public class TestingUpdateStatus : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string _fwStatusContent = "";
        string _macStatusContent = "";
        string _lanStatusContent = "";
        string _usbStatusContent = "";
        string _buttonStatusContent = "";
        string _ledStatusContent = "";
        string _errormessage = "";

        bool _fwExpander;
        bool _macExpander;
        bool _lanExpander;
        bool _usbExpander;
        bool _buttonExpander;
        bool _ledExpander;

        public bool fwExpander {
            get { return _fwExpander; }
            set {
                _fwExpander = value;
                OnPropertyChanged(nameof(fwExpander));
            }
        }
        public bool macExpander {
            get { return _macExpander; }
            set {
                _macExpander = value;
                OnPropertyChanged(nameof(macExpander));
            }
        }
        public bool lanExpander {
            get { return _lanExpander; }
            set {
                _lanExpander = value;
                OnPropertyChanged(nameof(lanExpander));
            }
        }
        public bool usbExpander {
            get { return _usbExpander; }
            set {
                _usbExpander = value;
                OnPropertyChanged(nameof(usbExpander));
            }
        }
        public bool buttonExpander {
            get { return _buttonExpander; }
            set {
                _buttonExpander = value;
                OnPropertyChanged(nameof(buttonExpander));
            }
        }
        public bool ledExpander {
            get { return _ledExpander; }
            set {
                _ledExpander = value;
                OnPropertyChanged(nameof(ledExpander));
            }
        }

        public string ERRORMESSAGE {
            get { return _errormessage; }
            set {
                _errormessage = value;
                OnPropertyChanged(nameof(ERRORMESSAGE));
            }
        }

        public string fwStatusContent {
            get { return _fwStatusContent; }
            set {
                _fwStatusContent = value;
                OnPropertyChanged(nameof(fwStatusContent));
            }
        }

        public string macStatusContent {
            get { return _macStatusContent; }
            set {
                _macStatusContent = value;
                OnPropertyChanged(nameof(macStatusContent));
            }
        }

        public string lanStatusContent {
            get { return _lanStatusContent; }
            set {
                _lanStatusContent = value;
                OnPropertyChanged(nameof(lanStatusContent));
            }
        }

        public string usbStatusContent {
            get { return _usbStatusContent; }
            set {
                _usbStatusContent = value;
                OnPropertyChanged(nameof(usbStatusContent));
            }
        }

        public string buttonStatusContent {
            get { return _buttonStatusContent; }
            set {
                _buttonStatusContent = value;
                OnPropertyChanged(nameof(buttonStatusContent));
            }
        }

        public string ledStatusContent {
            get { return _ledStatusContent; }
            set {
                _ledStatusContent = value;
                OnPropertyChanged(nameof(ledStatusContent));
            }
        }

        public bool flagFW {
            get { return Properties.Settings.Default.flag_FW == "1" ? true : false; }
            set {
                Properties.Settings.Default.flag_FW = value == true ? "1" : "0";
                OnPropertyChanged(nameof(flagFW));
            }
        }
        public bool flagMAC {
            get { return Properties.Settings.Default.flag_MAC == "1" ? true : false; }
            set {
                Properties.Settings.Default.flag_MAC = value == true ? "1" : "0";
                OnPropertyChanged(nameof(flagMAC));
            }
        }
        public bool flagLAN {
            get { return Properties.Settings.Default.flag_LAN == "1" ? true : false; }
            set {
                Properties.Settings.Default.flag_LAN = value == true ? "1" : "0";
                OnPropertyChanged(nameof(flagLAN));
            }
        }
        public bool flagUSB {
            get { return Properties.Settings.Default.flag_USB == "1" ? true : false; }
            set {
                Properties.Settings.Default.flag_USB = value == true ? "1" : "0";
                OnPropertyChanged(nameof(flagUSB));
            }
        }
        public bool flagBUTTON {
            get { return Properties.Settings.Default.flag_BUTTON == "1" ? true : false; }
            set {
                Properties.Settings.Default.flag_BUTTON = value == true ? "1" : "0";
                OnPropertyChanged(nameof(flagBUTTON));
            }
        }
        public bool flagLED {
            get { return Properties.Settings.Default.flag_LED == "1" ? true : false; }
            set {
                Properties.Settings.Default.flag_LED = value == true ? "1" : "0";
                OnPropertyChanged(nameof(flagLED));
            }
        }

        public void dontExpander() {
            this.fwExpander = false;
            this.macExpander = false;
            this.lanExpander = false;
            this.usbExpander = false;
            this.buttonExpander = false;
            this.ledExpander = false;
        }

    }

    public class PropertiesDefaultSetting : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string DUT_SerialPortName {
            get { return Properties.Settings.Default.DUT_SerialPortName; }
            set {
                Properties.Settings.Default.DUT_SerialPortName = value;
                OnPropertyChanged(nameof(DUT_SerialPortName));
            }
        }
        public string DUT_SerialBaudRate {
            get { return Properties.Settings.Default.DUT_SerialBaudRate; }
            set {
                Properties.Settings.Default.DUT_SerialBaudRate = value;
                OnPropertyChanged(nameof(DUT_SerialBaudRate));
            }
        }
        public string DUT_IPAddress {
            get { return Properties.Settings.Default.DUT_IPAddress; }
            set {
                Properties.Settings.Default.DUT_IPAddress = value;
                OnPropertyChanged(nameof(DUT_IPAddress));
            }
        }
        public string DUT_User {
            get { return Properties.Settings.Default.DUT_User; }
            set {
                Properties.Settings.Default.DUT_User = value;
                OnPropertyChanged(nameof(DUT_User));
            }
        }
        public string DUT_Pass {
            get { return Properties.Settings.Default.DUT_Pass; }
            set {
                Properties.Settings.Default.DUT_Pass = value;
                OnPropertyChanged(nameof(DUT_Pass));
            }
        }
        public string DUT_MACFormat {
            get { return Properties.Settings.Default.DUT_MACFormat; }
            set {
                Properties.Settings.Default.DUT_MACFormat = value;
                OnPropertyChanged(nameof(DUT_MACFormat));
            }
        }

        public void Save() {
            Properties.Settings.Default.Save();
        }

    }

    public class logInfomation {

        public string uploadFW { get; set; }
        public string writeMAC { get; set; }
        public string checkLAN { get; set; }
        public string checkUSB { get; set; }
        public string checkButton { get; set; }
        public string checkLED { get; set; }
        public string totalJud { get; set; }
        public string error { get; set; }
    }

    public class logDetailInfo {

        public string uploadFW { get; set; }
        public string writeMAC { get; set; }
        public string LANPort1 { get; set; }
        public string LANPort2 { get; set; }
        public string LANPort3 { get; set; }
        public string LANPort4 { get; set; }
        public string USB2 { get; set; }
        public string USB3 { get; set; }
        public string WPSButton { get; set; }
        public string ResetButton { get; set; }
        public string PowerLED { get; set; }
        public string PONLED { get; set; }
        public string INETLED { get; set; }
        public string WLANLED { get; set; }
        public string LAN1LED { get; set; }
        public string LAN2LED { get; set; }
        public string LAN3LED { get; set; }
        public string LAN4LED { get; set; }
        public string WPSLED { get; set; }
        public string LOSLED { get; set; }
        public string ERROR { get; set; }
        public string JUDGED { get; set; }
    }

}
