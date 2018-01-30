using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040E.Functions {
    public class exCheckUSB : baseFunctions {

        public bool checkUSBPortofDUT(out string _error) {
            _error = "";
            GlobalData.TestingContent.usbStatusContent = "=> Waiting...";
            try {
                //Wait DUT online
                if (!rewait_DUT_Online(GlobalData.checkUsbContent[0], out _error)) goto NG;
                //Wait DUT boot completed
                if (!wait_DUTWifiBootComplete(GlobalData.checkUsbContent[1], out _error)) goto NG;
                //check USB
                if (!check_USB(GlobalData.checkUsbContent[2], out _error)) goto NG;
                goto OK;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto NG;
            }
            NG:
            {
                GlobalData.TestingContent.usbStatusContent = "=> FAIL";
                return false;
            }
            OK:
            {
                GlobalData.TestingContent.usbStatusContent = "=> PASS";
                return true;
            }
        }
    }
}
