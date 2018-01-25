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
                System.IO.File.WriteAllText(rootPath + "cmd.txt", "tftp -i 192.168.1.1 put D:\\fw_GW040E\\tclinux.bin");
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

        /// <summary>
        /// WAIT DUT ONLINE
        /// </summary>
        /// <param name="u"></param>
        /// <param name="_error"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="_error"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="_error"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="_error"></param>
        /// <returns></returns>
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

    }
}
