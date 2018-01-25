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
