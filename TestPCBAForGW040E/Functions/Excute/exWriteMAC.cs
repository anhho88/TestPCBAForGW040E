using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestPCBAForGW040E.Functions {
    public class exWriteMAC : baseFunctions {

        public bool writeMACAddresstoDUT(out string _error) {
            _error = "";
            GlobalData.TestingContent.macStatusContent = "=> Waiting...";
            try {
                if (!get_MacAddress(GlobalData.writeMacContent[0], out _error)) goto NG;
                if (!wait_DUT_Online(GlobalData.writeMacContent[1], out _error)) goto NG;
                if (!wait_DUTBootComplete(GlobalData.writeMacContent[2], out _error)) goto NG;
                if (!login_toDUT(GlobalData.writeMacContent[3], out _error)) goto NG;
                if (!setMac_forEthernet0(GlobalData.writeMacContent[4], out _error)) goto NG;
                if (!wait_DUTBootComplete(GlobalData.writeMacContent[5], out _error)) goto NG;
                if (!login_toDUT(GlobalData.writeMacContent[6], out _error)) goto NG;
                if (!confirm_MacAddress(GlobalData.writeMacContent[7], out _error)) goto NG;
                goto OK;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto NG;
            }

            NG:
            {
                GlobalData.TestingContent.macStatusContent = "=> FAIL";
                return false;
            }
            OK:
            {
                GlobalData.TestingContent.macStatusContent = "=> PASS";
                return true;
            }
        }
    }
}
