using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestPCBAForGW040E.Functions
{
    public class mainProgram
    {
        public bool Excuted(out string _error) {
            _error = "";
            GlobalData.logContent.logviewUART = "";
            GlobalData.mainwindowInformation.statustestContent = "Waiting...";
            GlobalData.mainwindowInformation.IsTesting = true;
            GlobalData.logResult = new logInfomation();
            GlobalData.TestingContent.dontExpander();
            GlobalData.logDetailResult = new logDetailInfo();

            var settings = Properties.Settings.Default;
            //Upload Firmware
            string errorMessage;
            if (settings.flag_FW == "1") {
                GlobalData.TestingContent.fwExpander = true;
                if (!(new exUploadFW().uploadFWtoDUT(out errorMessage))) { GlobalData.logResult.uploadFW = "FAIL"; goto NG; }
                else { GlobalData.logResult.uploadFW = "PASS"; GlobalData.TestingContent.fwExpander = false; }
            } else GlobalData.logResult.uploadFW = "None";
            //Write MAC Address
            if (settings.flag_MAC =="1") {
                GlobalData.TestingContent.macExpander = true;
                if (!(new exWriteMAC().writeMACAddresstoDUT(out errorMessage))) { GlobalData.logResult.writeMAC = "FAIL"; goto NG; }
                else { GlobalData.logResult.writeMAC = "PASS"; GlobalData.TestingContent.macExpander = false; }
                } else GlobalData.logResult.writeMAC = "None";
            //Check LAN ports
            if (settings.flag_LAN == "1") {
                GlobalData.TestingContent.lanExpander = true;
                if (!(new exCheckLAN().checkLANPortofDUT(out errorMessage))) { GlobalData.logResult.checkLAN = "FAIL"; goto NG; }
                else { GlobalData.logResult.checkLAN = "PASS"; GlobalData.TestingContent.lanExpander = false; }
                } else GlobalData.logResult.checkLAN = "None";
            //Check USB
            if (settings.flag_USB == "1") {
                GlobalData.TestingContent.usbExpander = true;
                if (!(new exCheckUSB().checkUSBPortofDUT(out errorMessage))) { GlobalData.logResult.checkUSB = "FAIL"; goto NG; }
                else { GlobalData.logResult.checkUSB = "PASS"; GlobalData.TestingContent.usbExpander = false; }
                } else GlobalData.logResult.checkUSB = "None";
            //Check Button
            if (settings.flag_BUTTON == "1") {
                GlobalData.TestingContent.buttonExpander = true;
                if (!(new exCheckButton().checkButtonOfDUT(out errorMessage))) { GlobalData.logResult.checkButton = "FAIL"; goto NG; }
                else { GlobalData.logResult.checkButton = "PASS"; GlobalData.TestingContent.buttonExpander = false; }
                } else GlobalData.logResult.checkButton = "None";
            //Check LEDs
            if (settings.flag_LED == "1") {
                GlobalData.TestingContent.ledExpander = true;
                if (!(new exCheckLED().checkLEDStateOfDUT(out errorMessage))) { GlobalData.logResult.checkLED = "FAIL"; goto NG; }
                else { GlobalData.logResult.checkLED = "PASS"; GlobalData.TestingContent.ledExpander = false; }
                } else GlobalData.logResult.checkLED = "None";

            goto OK;
            NG:
            {
                GlobalData.mainwindowInformation.statustestContent = "NG";
                GlobalData.mainwindowInformation.IsTesting = false;
                GlobalData.TestingContent.ERRORMESSAGE = string.Format("ERROR MESSAGE: {0}", errorMessage);
                GlobalData.logResult.totalJud = "NG";
                GlobalData.logResult.error = errorMessage.Replace("\r", "").Replace("\n", "");
                GlobalData.logDetailResult.JUDGED = GlobalData.logResult.totalJud;
                GlobalData.logDetailResult.ERROR = GlobalData.logResult.error;
                GlobalData.logDetailResult.uploadFW = GlobalData.logResult.uploadFW;
                GlobalData.logDetailResult.writeMAC = GlobalData.logResult.writeMAC;
                //Read MAC address
                bool ret = new exGetMAC().getMACAddressOfDUT();
                //Save log
                ret = new logManagement().saveLog();
                ret = new logManagement().saveLogDetail();
                return false;
            }
            OK:
            {
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
                return true;
            }
            
        }

    }
}
