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
            GlobalData.mainwindowInformation.statustestContent = "Waiting...";
            GlobalData.mainwindowInformation.IsTesting = true;
            var settings = Properties.Settings.Default;
            Thread.Sleep(1000);
            goto OK;
            ////Upload FW
            //string errorMessage;
            //if (settings.flag_FW=="1") {
            //    if (!(new exUploadFW().uploadFWtoDUT(out errorMessage))) return;
            //}
            OK:
            GlobalData.mainwindowInformation.statustestContent = "OK";
            GlobalData.mainwindowInformation.IsTesting = false;
            return true;
        }

    }
}
