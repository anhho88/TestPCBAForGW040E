using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace TestPCBAForGW040E.Functions {
    public abstract class baseFunctions {

        waitDUT wait = null;
        GetMAC gm = null;
        PlugLAN pl = null;
        PressWPS pw = null;
        LED led = null;

        private void callWaiDUT(string timeout) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    wait = new waitDUT(timeout);
                    wait.ShowDialog();
                }));
            }
            catch { }
        }

        private void destroyWaitDUT() {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    wait.Close();
                }));
            }
            catch { }
        }

        private void callGetMAC(int retry, int timeout) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    gm = new GetMAC(retry, timeout);
                    gm.ShowDialog();
                }));
            }
            catch { }
        }

        private void destroyGetMAC() {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    gm.Close();
                }));
            }
            catch { }
        }

        private void callPlugLAN(int inOut, int timeout) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    pl = new PlugLAN(inOut, timeout);
                    pl.ShowDialog();
                }));
            }
            catch { }
        }

        private void destroyPlugLAN() {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    pl.Close();
                }));
            }
            catch { }
        }

        private void callPressWPS(int title, int timeout) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    pw = new PressWPS(title, timeout);
                    pw.ShowDialog();
                }));
            }
            catch { }
        }

        private void destroyPressWPS() {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    pw.Close();
                }));
            }
            catch { }
        }

        private void callLED(int timeout) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    led = new LED(timeout);
                    led.ShowDialog();
                }));
            }
            catch { }
        }

        private void destroyLED() {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    led.Close();
                }));
            }
            catch { }
        }

        
        private bool accessDUT(out string message) {
            message = "";
            try {
                GlobalData.serialPort.Port.Write("b");
                Thread.Sleep(100);
                return GlobalData.logContent.logviewUART.Contains("bldr>") == true ? true : false;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

        private bool setFTPServerIPAddress(out string message) {
            message = "";
            try {
                GlobalData.serialPort.Port.Write("ipaddr 192.168.1.1\r\n");
                Thread.Sleep(100);
                return GlobalData.logContent.logviewUART.Contains("Change IP address to 192.168.1.1") == true ? true : false;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

        private bool putFirwareThroughWPS(out string message) {
            message = "";
            try {
                string rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
                System.IO.File.WriteAllText(rootPath + "cmd.txt", string.Format("tftp -i 192.168.1.1 put {0}", GlobalData.defaultSettings.FWPath));
                try {
                    System.IO.File.Delete(rootPath + "wps.txt");
                }
                catch { }
                Thread.Sleep(100);
                ProcessStartInfo psi = new ProcessStartInfo(rootPath + "RunPowerShell.exe");
                Thread.Sleep(100);
                psi.UseShellExecute = true;
                Thread.Sleep(100);
                Process.Start(psi);
                Thread.Sleep(100);
                return true;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

        protected bool sendDataToDUT(string data) {
            try {
                GlobalData.serialPort.Port.Write(data);
                return true;
            }
            catch {
                return false;
            }
        }

        private bool stringRegexMatch(string input, string pattern) {

            input = input.ToLower();
            pattern = pattern.ToLower();
            string[] inputs = input.ToCharArray().Select(c => c.ToString()).ToArray();
            string[] patterns = pattern.ToCharArray().Select(c => c.ToString()).ToArray();

            //Compare string
            if (input.Length != pattern.Length) return false;
            for (int i = 0; i < input.Length; i++) {
                if (patterns[i] == "#") {
                    int r;
                    bool ret = int.TryParse(inputs[i], out r);
                    if (!ret) return false;
                }
                else {
                    if (inputs[i] != patterns[i]) return false;
                }
            }
            return true;
        }

        protected bool wait_DUT_Online(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                callWaiDUT(tOut.ToString());
                while (!_flag) {
                    GlobalData.logContent.logviewUART = "";
                    while (GlobalData.logContent.logviewUART.Length == 0) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyWaitDUT();
                    content.ACTUAL = (index * 1000).ToString();
                    if (index < (tOut / 1000)) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool access_toUboot(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    while (!GlobalData.logContent.logviewUART.Contains("Press any key in 3 secs to enter boot command mode.")) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    if (index >= (tOut / 1000)) {
                        content.ACTUAL = (index * 1000).ToString();
                        break;
                    }
                    int rep = 0;
                    REPEAT:
                    if (!this.accessDUT(out _error)) {
                        if (rep < tRetry) { rep++; goto REPEAT; }
                        else { content.ACTUAL = "Error"; break; }
                    }
                    else {
                        content.ACTUAL = stvalue;
                        _flag = true;
                        break;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool set_FTPServer_IPAddress(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    if (!this.setFTPServerIPAddress(out _error)) {
                        if (index >= tRetry) { _error = "Request time out."; break; }
                        else index++;
                    }
                    else {
                        content.ACTUAL = stvalue;
                        _flag = true;
                        break;
                    }
                }
                if (!_flag) content.ACTUAL = "Error";
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool putFirm_ThroughWPS(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    if (!this.putFirwareThroughWPS(out _error)) {
                        if (index >= tRetry) break;
                        else index++;
                    }
                    else {
                        index = 0;
                        while (!System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "wps.txt")) {
                            Thread.Sleep(1000);
                            if (index >= (tOut / 1000)) break;
                            else index++;
                        }
                        if (index >= (tOut / 1000)) { _error = "Request time out."; break; }
                        string tmpStr = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "wps.txt");
                        GlobalData.logContent.logviewWPS = tmpStr;
                        if (tmpStr.ToUpper().Contains("ERROR") || tmpStr.Trim().Replace("\n", "").Replace("\r", "") == string.Empty) {
                            _error = tmpStr;
                            break;
                        }
                        bool _end = false;
                        while (!_end) {
                            int t1 = GlobalData.logContent.logviewUART.Length ;
                            Thread.Sleep(200);
                            int t2 = GlobalData.logContent.logviewUART.Length;
                            Thread.Sleep(200);
                            int t3 = GlobalData.logContent.logviewUART.Length;
                            Thread.Sleep(200);
                            if (t1 == t2 && t2 == t3) break;
                        }
                        if (GlobalData.logContent.logviewUART.ToUpper().Contains("STARTING THE TFTP DOWNLOAD") &&
                            GlobalData.logContent.logviewUART.ToUpper().Contains("CHECK DATA SUCCESS, PREPARE TO UPLOAD")) {
                            content.ACTUAL = stvalue;
                            _flag = true;
                            break;
                        }
                        else {
                            break;
                        }
                    }
                }
                if (!_flag) content.ACTUAL = "Error";
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool get_MacAddress(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                callGetMAC(tRetry, tOut);
                while (!_flag) {
                    GlobalData.macAddress = "";
                    while (GlobalData.macAddress.Length == 0) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyGetMAC();
                    content.ACTUAL = (index * 1000).ToString();
                    if (index < (tOut / 1000)) {
                        if (GlobalData.macAddress.Length == 12) _flag = true;
                        else _error = "Mac Address sai định dạng!";
                        break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool wait_DUTBootComplete(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            GlobalData.uartData = "";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    bool _end = false;
                    while (!_end) {
                        if (GlobalData.uartData.Contains("Please press Enter to activate this console")) break;
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    content.ACTUAL = (index * 1000).ToString();
                    if (index < (tOut / 1000)) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool login_toDUT(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    this.sendDataToDUT("\r\n");
                    while (!GlobalData.logContent.logviewUART.Contains("tc login:")) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    if (index >= (tOut / 1000)) { _error = "Request time out"; break; }
                    this.sendDataToDUT(GlobalData.defaultSettings.DUT_User + "\n");
                    while (!GlobalData.logContent.logviewUART.Contains("Password:")) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    if (index >= (tOut / 1000)) { _error = "Request time out"; break; }
                    this.sendDataToDUT(GlobalData.defaultSettings.DUT_Pass + "\n");
                    while (!GlobalData.logContent.logviewUART.Contains(stvalue)) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    if (index >= (tOut / 1000)) {
                        content.ACTUAL = (index * 1000).ToString();
                        _error = "Request time out";
                        break;
                    }
                    else {
                        content.ACTUAL = stvalue;
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool setMac_forEthernet0(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    this.sendDataToDUT(string.Format("sys mac {0}\n", GlobalData.macAddress));
                    string st = string.Format("new mac addr = {0}:{1}:{2}:{3}:{4}:{5}",
                        GlobalData.macAddress.Substring(0, 2).ToLower(),
                        GlobalData.macAddress.Substring(2, 2).ToLower(),
                        GlobalData.macAddress.Substring(4, 2).ToLower(),
                        GlobalData.macAddress.Substring(6, 2).ToLower(),
                        GlobalData.macAddress.Substring(8, 2).ToLower(),
                        GlobalData.macAddress.Substring(10, 2).ToLower()
                        );
                    while (!GlobalData.logContent.logviewUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    if (index >= (tOut / 1000)) {
                        content.ACTUAL = (index * 1000).ToString();
                        _error = "Request time out";
                        break;
                    }
                    else {
                        content.ACTUAL = st;
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                GlobalData.logContent.logviewUART = "";
                return _flag;
            }
        }

        protected bool confirm_MacAddress(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    this.sendDataToDUT(string.Format("ifconfig\n"));
                    string st = string.Format("Link encap:Ethernet  HWaddr {0}:{1}:{2}:{3}:{4}:{5}",
                       GlobalData.macAddress.Substring(0, 2),
                       GlobalData.macAddress.Substring(2, 2),
                       GlobalData.macAddress.Substring(4, 2),
                       GlobalData.macAddress.Substring(6, 2),
                       GlobalData.macAddress.Substring(8, 2),
                       GlobalData.macAddress.Substring(10, 2)
                       );
                    while (!GlobalData.logContent.logviewUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    if (index >= (tOut / 1000)) {
                        content.ACTUAL = (index * 1000).ToString();
                        _error = "Request time out";
                        break;
                    }
                    else {
                        content.ACTUAL = st;
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }


        protected bool read_MacAddress(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.uartData = "";
                while (!_flag) {
                    this.sendDataToDUT(string.Format("ifconfig\n"));
                    string st = string.Format("Link encap:Ethernet  HWaddr");
                    while (!GlobalData.uartData.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    if (index >= (tOut / 1000)) {
                        content.ACTUAL = (index * 1000).ToString();
                        _error = "Request time out";
                        break;
                    }
                    else {
                        string tmpStr = GlobalData.uartData;
                        string[] buffer = tmpStr.Split(new string[] { "HWaddr" }, StringSplitOptions.None);
                        tmpStr = buffer[1].Replace("\r", "").Replace("\n", "").Trim();
                        string mac = tmpStr.Substring(0, 17);
                        GlobalData.macAddress = mac.Replace(":", "");
                        content.ACTUAL = st;
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }


        protected bool rewait_DUT_Online(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                //////////////////////////////////////
                if (GlobalData.logContent.logviewUART.Length != 0) {
                    _flag = true;
                    content.ACTUAL = "0";
                    goto END;
                }
                //////////////////////////////////////
                callWaiDUT(tOut.ToString());
                while (!_flag) {
                    GlobalData.logContent.logviewUART = "";
                    while (GlobalData.logContent.logviewUART.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyWaitDUT();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool rewait_DUTBootComplete(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            GlobalData.uartData = "";
            //////////////////////////////////////
            if (GlobalData.logContent.logviewUART.Contains("Please press Enter to activate this console") == true) {
                _flag = true;
                content.ACTUAL = "0";
                goto END;
            }
            //////////////////////////////////////
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    bool _end = false;
                    while (!_end) {
                        if (GlobalData.uartData.Contains("Please press Enter to activate this console")) break;
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    content.ACTUAL = (index * 1000).ToString();
                    if (index < (tOut / 1000)) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool pluginLANPort(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.lanResult = "";
                callPlugLAN(0, tOut);
                while (!_flag) {
                    while (GlobalData.lanResult.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyPlugLAN();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) {
                        if (GlobalData.lanResult == "OK") _flag = true;
                        else break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool plugoutLANPort(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.lanResult = "";
                callPlugLAN(1, tOut);
                while (!_flag) {
                    while (GlobalData.lanResult.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyPlugLAN();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) {
                        if (GlobalData.lanResult == "OK") _flag = true;
                        else break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool wait_DUTWifiBootComplete(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            GlobalData.uartData = "";
            //////////////////////////////////////
            for (int i = 0; i < 10; i++) {
                string pattern = string.Format("br0: port {0}(ra0) entering forwarding state", i);
                if (GlobalData.logContent.logviewUART.Contains(pattern) == true) {
                    _flag = true;
                    content.ACTUAL = "0";
                    goto END;
                }
            }
            
            //////////////////////////////////////
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                while (!_flag) {
                    bool _end = false;
                    while (!_end) {
                        bool ret = false;
                        for (int i = 0; i < 10; i++) {
                            string pattern = string.Format("br0: port {0}(ra0) entering forwarding state", i);
                            if (GlobalData.uartData.Contains(pattern) == true) { ret = true; break; }
                        }
                        if (ret == true) break;
                        Thread.Sleep(1000);
                        if (index >= (tOut / 1000)) break;
                        else index++;
                    }
                    content.ACTUAL = (index * 1000).ToString();
                    if (index < (tOut / 1000)) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool check_WPSbutton(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.buttonResult = "";
                callPressWPS(0, tOut);
                while (!_flag) {
                    while (GlobalData.buttonResult.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyPressWPS();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) {
                        if (GlobalData.buttonResult == "OK") _flag = true;
                        else break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool check_Resetbutton(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.buttonResult = "";
                callPressWPS(1, tOut);
                while (!_flag) {
                    while (GlobalData.buttonResult.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyPressWPS();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) {
                        if (GlobalData.buttonResult == "OK") _flag = true;
                        else break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool check_USB(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.usbResult = "";
                callPressWPS(2, tOut);
                while (!_flag) {
                    while (GlobalData.usbResult.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyPressWPS();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) {
                        if (GlobalData.usbResult == "OK") _flag = true;
                        else break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

        protected bool check_LEDs(ContentGridFields content, out string _error) {
            _error = "";
            bool _flag = false;
            content.JUDGED = "waiting...";
            try {
                string stvalue = content.STANDARD;
                int tOut = content.TIMEOUT == "-" ? 1000 : int.Parse(content.TIMEOUT);
                int tRetry = content.RETRY == "-" ? 0 : int.Parse(content.RETRY);
                int index = 0;
                GlobalData.ledResult = "";
                callLED(tOut);
                while (!_flag) {
                    while (GlobalData.ledResult.Length == 0) {
                        Thread.Sleep(100);
                        if (index >= (tOut / 100)) { _error = "Request time out."; break; }
                        else index++;
                    }
                    destroyLED();
                    content.ACTUAL = (index * 100).ToString();
                    if (index < (tOut / 100)) {
                        if (GlobalData.ledResult == "OK") _flag = true;
                        else break;
                    }
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                content.JUDGED = _flag == true ? "PASS" : "FAIL";
                return _flag;
            }
        }

    }
}
