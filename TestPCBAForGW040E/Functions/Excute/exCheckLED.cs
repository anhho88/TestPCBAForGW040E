using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040E.Functions {
    public class exCheckLED : baseFunctions {

        public bool checkLEDStateOfDUT (out string _error) {
            _error = "";
            GlobalData.TestingContent.ledStatusContent = "=> Waiting...";
            try {
                //check LED
                if (!check_LEDs(GlobalData.checkLedContent[0], out _error)) goto NG;
                goto OK;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto NG;
            }
            NG:
            {
                GlobalData.TestingContent.ledStatusContent = "=> FAIL";
                return false;
            }
            OK:
            {
                GlobalData.TestingContent.ledStatusContent = "=> PASS";
                return true;
            }
        }
    }
}
