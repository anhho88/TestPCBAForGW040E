using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace TestPCBAForGW040E.Functions {


    public class exUploadFW : baseFunctions {

        public bool uploadFWtoDUT(out string _error) {
            _error = "";
            GlobalData.TestingContent.fwStatusContent = "=> Waiting...";
            try {
                if (!wait_DUT_Online(GlobalData.uploadFWContent[0], out _error)) goto NG;
                if (!access_toUboot(GlobalData.uploadFWContent[1], out _error)) goto NG;
                if (!set_FTPServer_IPAddress(GlobalData.uploadFWContent[2], out _error)) goto NG;
                if (!putFirm_ThroughWPS(GlobalData.uploadFWContent[3], out _error)) goto NG;
                //MessageBox.Show("Success!...");
                var settings = Properties.Settings.Default;
                if (settings.flag_MAC == "1" || settings.flag_LAN == "1" || settings.flag_USB == "1" || settings.flag_BUTTON == "1") {

                    Thread.Sleep(1000);
                    sendDataToDUT("\r");
                    Thread.Sleep(500);
                    sendDataToDUT("go\r\n");
                    Thread.Sleep(100);
                }
                goto OK;
            } catch (Exception ex) {
                _error = ex.ToString();
                goto NG;
            }
            NG:
            {
                GlobalData.TestingContent.fwStatusContent = "=> FAIL";
                return false;
            }
            OK:
            {
                GlobalData.TestingContent.fwStatusContent = "=> PASS";
                return true;
            }
        }
    }
}
