using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestPCBAForGW040E.Functions {
    public class mainProgram {

        void initParameters() {
            GlobalData.logContent.logviewUART = "";
            GlobalData.mainwindowInformation.statustestContent = "Waiting...";
            GlobalData.mainwindowInformation.IsTesting = true;
            GlobalData.logResult = new logInfomation();
            //GlobalData.TestingContent.dontExpander();
            GlobalData.logDetailResult = new logDetailInfo();
            var settings = Properties.Settings.Default;
            if (settings.flag_FW == "1") {
                GlobalData.logResult.uploadFW = "-";
                GlobalData.logDetailResult.uploadFW = "-";
            }
            else {
                GlobalData.logResult.uploadFW = "None";
                GlobalData.logDetailResult.uploadFW = "None";
            }
            if (settings.flag_MAC == "1") {
                GlobalData.logResult.writeMAC = "-";
                GlobalData.logDetailResult.writeMAC = "-";
            }
            else {
                GlobalData.logResult.writeMAC = "None";
                GlobalData.logDetailResult.writeMAC = "None";
            }
            if (settings.flag_LAN == "1") {
                GlobalData.logResult.checkLAN = "-";
                GlobalData.logDetailResult.LANPort1 = "-";
                GlobalData.logDetailResult.LANPort2 = "-";
                GlobalData.logDetailResult.LANPort3 = "-";
                GlobalData.logDetailResult.LANPort4 = "-";
            }
            else {
                GlobalData.logResult.checkLAN = "None";
                GlobalData.logDetailResult.LANPort1 = "None";
                GlobalData.logDetailResult.LANPort2 = "None";
                GlobalData.logDetailResult.LANPort3 = "None";
                GlobalData.logDetailResult.LANPort4 = "None";
            }
            if (settings.flag_USB == "1") {
                GlobalData.logResult.checkUSB = "-";
                GlobalData.logDetailResult.USB2 = "-";
                GlobalData.logDetailResult.USB3 = "-";
            }
            else {
                GlobalData.logResult.checkUSB = "None";
                GlobalData.logDetailResult.USB2 = "None";
                GlobalData.logDetailResult.USB3 = "None";
            }
            if (settings.flag_BUTTON == "1") {
                GlobalData.logResult.checkButton = "-";
                GlobalData.logDetailResult.WPSButton = "-";
                GlobalData.logDetailResult.ResetButton = "-";
            }
            else {
                GlobalData.logResult.checkButton = "None";
                GlobalData.logDetailResult.WPSButton = "None";
                GlobalData.logDetailResult.ResetButton = "None";
            }
            if (settings.flag_LED == "1") {
                GlobalData.logResult.checkLED = "-";
                GlobalData.logDetailResult.PowerLED = "-";
                GlobalData.logDetailResult.PONLED = "-";
                GlobalData.logDetailResult.INETLED = "-";
                GlobalData.logDetailResult.WLANLED = "-";
                GlobalData.logDetailResult.LAN1LED = "-";
                GlobalData.logDetailResult.LAN2LED = "-";
                GlobalData.logDetailResult.LAN3LED = "-";
                GlobalData.logDetailResult.LAN4LED = "-";
                GlobalData.logDetailResult.WPSLED = "-";
                GlobalData.logDetailResult.LOSLED = "-";
            }
            else {
                GlobalData.logResult.checkLED = "None";
                GlobalData.logDetailResult.PowerLED = "None";
                GlobalData.logDetailResult.PONLED = "None";
                GlobalData.logDetailResult.INETLED = "None";
                GlobalData.logDetailResult.WLANLED = "None";
                GlobalData.logDetailResult.LAN1LED = "None";
                GlobalData.logDetailResult.LAN2LED = "None";
                GlobalData.logDetailResult.LAN3LED = "None";
                GlobalData.logDetailResult.LAN4LED = "None";
                GlobalData.logDetailResult.WPSLED = "None";
                GlobalData.logDetailResult.LOSLED = "None";
            }
        }

        void judOK() {
            GlobalData.mainwindowInformation.statustestContent = "OK";
            GlobalData.mainwindowInformation.IsTesting = false;
            GlobalData.logResult.totalJud = "OK";
            GlobalData.logResult.error = "None";
            GlobalData.logDetailResult.JUDGED = GlobalData.logResult.totalJud;
            GlobalData.logDetailResult.ERROR = GlobalData.logResult.error;
            GlobalData.logDetailResult.uploadFW = GlobalData.logResult.uploadFW;
            GlobalData.logDetailResult.writeMAC = GlobalData.logResult.writeMAC;
            //Read MAC address
            bool ret = new exGetMAC().getMACAddressOfDUT();
            //Save log
            ret = new logManagement().saveLog();
            ret = new logManagement().saveLogDetail();
        }

        void judNG() {
            GlobalData.mainwindowInformation.statustestContent = "NG";
            GlobalData.mainwindowInformation.IsTesting = false;
            GlobalData.logResult.totalJud = "NG";
            GlobalData.logDetailResult.JUDGED = GlobalData.logResult.totalJud;
            GlobalData.logDetailResult.ERROR = GlobalData.logResult.error;
            GlobalData.logDetailResult.uploadFW = GlobalData.logResult.uploadFW;
            GlobalData.logDetailResult.writeMAC = GlobalData.logResult.writeMAC;
            //Read MAC address
            bool ret = new exGetMAC().getMACAddressOfDUT();
            //Save log
            ret = new logManagement().saveLog();
            ret = new logManagement().saveLogDetail();
        }


        public bool Excuted(out string _error) {
            _error = "";
            initParameters();
            var settings = Properties.Settings.Default;
            //Upload Firmware
            string errorMessage;
            if (settings.flag_FW == "1") {
                //GlobalData.TestingContent.fwExpander = true;
                if (!(new exUploadFW().uploadFWtoDUT(out errorMessage))) { GlobalData.logResult.uploadFW = "FAIL"; goto NG; }
                else {
                    GlobalData.logResult.uploadFW = "PASS";
                    //GlobalData.TestingContent.fwExpander = false;
                }
            }
            //Write MAC Address
            if (settings.flag_MAC == "1") {
                //GlobalData.TestingContent.macExpander = true;
                if (!(new exWriteMAC().writeMACAddresstoDUT(out errorMessage))) { GlobalData.logResult.writeMAC = "FAIL"; goto NG; }
                else {
                    GlobalData.logResult.writeMAC = "PASS";
                    //GlobalData.TestingContent.macExpander = false;
                }
            }
            //Check LAN ports
            if (settings.flag_LAN == "1") {
                //GlobalData.TestingContent.lanExpander = true;
                if (!(new exCheckLAN().checkLANPortofDUT(out errorMessage))) { GlobalData.logResult.checkLAN = "FAIL"; goto NG; }
                else {
                    GlobalData.logResult.checkLAN = "PASS";
                    //GlobalData.TestingContent.lanExpander = false;
                }
            }
            //Check USB
            if (settings.flag_USB == "1") {
                //GlobalData.TestingContent.usbExpander = true;
                if (!(new exCheckUSB().checkUSBPortofDUT(out errorMessage))) { GlobalData.logResult.checkUSB = "FAIL"; goto NG; }
                else {
                    GlobalData.logResult.checkUSB = "PASS";
                    //GlobalData.TestingContent.usbExpander = false;
                }
            }
            //Check Button
            if (settings.flag_BUTTON == "1") {
                //GlobalData.TestingContent.buttonExpander = true;
                if (!(new exCheckButton().checkButtonOfDUT(out errorMessage))) { GlobalData.logResult.checkButton = "FAIL"; goto NG; }
                else {
                    GlobalData.logResult.checkButton = "PASS";
                    //GlobalData.TestingContent.buttonExpander = false;
                }
            }
            //Check LEDs
            if (settings.flag_LED == "1") {
                //GlobalData.TestingContent.ledExpander = true;
                if (!(new exCheckLED().checkLEDStateOfDUT(out errorMessage))) { GlobalData.logResult.checkLED = "FAIL"; goto NG; }
                else {
                    GlobalData.logResult.checkLED = "PASS";
                    //GlobalData.TestingContent.ledExpander = false;
                }
            }

            goto OK;
            NG:
            {
                GlobalData.TestingContent.ERRORMESSAGE = string.Format("ERROR MESSAGE: {0}", errorMessage);
                GlobalData.logResult.error = errorMessage.Replace("\r", "").Replace("\n", "");
                judNG();
                return false;
            }
            OK:
            {
                judOK();
                return true;
            }

        }

    }
}
