using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040E.Functions {
    public class exCheckButton : baseFunctions {

        public bool checkButtonOfDUT(out string _error) {
            _error = "";
            GlobalData.TestingContent.buttonStatusContent = "=> Waiting...";
            try {
                //Wait DUT online
                if (!rewait_DUT_Online(GlobalData.checkButtonContent[0], out _error)) goto NG;
                //Wait DUT boot completed
                if (!wait_DUTWifiBootComplete(GlobalData.checkButtonContent[1], out _error)) goto NG;
                goto OK;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto NG;
            }
            NG:
            {
                GlobalData.TestingContent.buttonStatusContent = "=> FAIL";
                return false;
            }
            OK:
            {
                GlobalData.TestingContent.buttonStatusContent = "=> PASS";
                return true;
            }
        }
    }
}
