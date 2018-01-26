using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040E.Functions {
    public class exCheckLAN : baseFunctions {

        public bool checkLANPortofDUT(out string _error) {
            _error = "";
            GlobalData.TestingContent.lanStatusContent = "=> Waiting...";
            try {
                //Wait DUT online
                if (!rewait_DUT_Online(GlobalData.checkLanContent[0], out _error)) goto NG;
                //Wait DUT boot completed
                if (!rewait_DUTBootComplete(GlobalData.checkLanContent[1], out _error)) goto NG;
                //Plug in LAN
                if (!pluginLANPort(GlobalData.checkLanContent[2], out _error)) goto NG;
                //Plug out LAN
                if (!plugoutLANPort(GlobalData.checkLanContent[3], out _error)) goto NG;
                goto OK;
            } 
            catch (Exception ex) {
                _error = ex.ToString();
                goto NG;
            }
            NG:
            {
                GlobalData.TestingContent.lanStatusContent = "=> FAIL";
                return false;
            }
            OK:
            {
                GlobalData.TestingContent.lanStatusContent = "=> PASS";
                return true;
            }
        }

    }
}
