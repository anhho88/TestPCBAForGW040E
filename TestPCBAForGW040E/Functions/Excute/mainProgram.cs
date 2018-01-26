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
            var settings = Properties.Settings.Default;
            //Upload Firmware
            string errorMessage;
            if (settings.flag_FW == "1") {
                if (!(new exUploadFW().uploadFWtoDUT(out errorMessage))) goto NG;
            }
            //Write MAC Address
            if (settings.flag_MAC =="1") {
                if (!(new exWriteMAC().writeMACAddresstoDUT(out errorMessage))) goto NG;
            }
            //Check LAN ports
            if(settings.flag_LAN == "1") {
                if (!(new exCheckLAN().checkLANPortofDUT(out errorMessage))) goto NG;
            }
            //Check USB
            if(settings.flag_USB == "1") {
                if (!(new exCheckUSB().checkUSBPortofDUT(out errorMessage))) goto NG;
            }
            //Check Button
            if (settings.flag_BUTTON == "1") {
                if (!(new exCheckButton().checkButtonOfDUT(out errorMessage))) goto NG;
            }
            //Check LEDs
            if (settings.flag_LED == "1") {

            } 
            goto OK;
            NG:
            {
                GlobalData.mainwindowInformation.statustestContent = "NG";
                GlobalData.mainwindowInformation.IsTesting = false;
                return false;
            }
            OK:
            {
                GlobalData.mainwindowInformation.statustestContent = "OK";
                GlobalData.mainwindowInformation.IsTesting = false;
                return true;
            }
            
        }

    }
}
