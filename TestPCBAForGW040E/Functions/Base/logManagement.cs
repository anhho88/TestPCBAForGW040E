using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace TestPCBAForGW040E.Functions {
    public class logManagement {

        public bool saveLog() {
            try {
                string directPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string folderPath = string.Format("{0}\\log", directPath);
                string fileName = string.Format("{0}_{1}.csv", GlobalData.macAddress, DateTime.Now.ToString("HHmmss"));
                if (!System.IO.Directory.Exists(folderPath)) {
                    System.IO.Directory.CreateDirectory(folderPath);
                    Thread.Sleep(100);
                }
                if (!System.IO.File.Exists(string.Format("{0}\\{1}", folderPath, fileName))) {
                    System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(string.Format("{0}\\{1}", folderPath, fileName));
                    streamwriter.WriteLine("Key,Value");
                    streamwriter.WriteLine(string.Format("PCName,{0}", System.Environment.MachineName.ToString()));
                    streamwriter.WriteLine(string.Format("DateTime,{0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                    streamwriter.WriteLine(string.Format("UploadFW,{0}", GlobalData.logResult.uploadFW));
                    streamwriter.WriteLine(string.Format("WriteMAC,{0}", GlobalData.logResult.writeMAC));
                    streamwriter.WriteLine(string.Format("CheckLAN,{0}", GlobalData.logResult.checkLAN));
                    streamwriter.WriteLine(string.Format("CheckUSB,{0}", GlobalData.logResult.checkUSB));
                    streamwriter.WriteLine(string.Format("CheckButtons,{0}", GlobalData.logResult.checkButton));
                    streamwriter.WriteLine(string.Format("CheckLEDs,{0}", GlobalData.logResult.checkLED));
                    streamwriter.WriteLine(string.Format("Error,{0}", GlobalData.logResult.error));
                    streamwriter.WriteLine(string.Format("TotalJudged,{0}", GlobalData.logResult.totalJud));
                    streamwriter.Dispose();
                    streamwriter.Close();
                }
                return true;
            } catch {
                return false;
            }
        }

        public bool saveLogDetail() {
            try {
                string directPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string folderPath = string.Format("{0}\\logDetail", directPath);
                string fileName = string.Format("{0}_{1}.csv", GlobalData.macAddress, DateTime.Now.ToString("HHmmss"));
                if (!System.IO.Directory.Exists(folderPath)) {
                    System.IO.Directory.CreateDirectory(folderPath);
                    Thread.Sleep(100);
                }
                if (!System.IO.File.Exists(string.Format("{0}\\{1}", folderPath, fileName))) {
                    System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(string.Format("{0}\\{1}", folderPath, fileName));
                    streamwriter.WriteLine("Key,Value");
                    streamwriter.WriteLine(string.Format("PCName,{0}", System.Environment.MachineName.ToString()));
                    streamwriter.WriteLine(string.Format("DateTime,{0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                    streamwriter.WriteLine(string.Format("UploadFW,{0}", GlobalData.logDetailResult.uploadFW));
                    streamwriter.WriteLine(string.Format("WriteMAC,{0}", GlobalData.logDetailResult.writeMAC));
                    streamwriter.WriteLine(string.Format("LANPort1,{0}", GlobalData.logDetailResult.LANPort1));
                    streamwriter.WriteLine(string.Format("LANPort2,{0}", GlobalData.logDetailResult.LANPort2));
                    streamwriter.WriteLine(string.Format("LANPort3,{0}", GlobalData.logDetailResult.LANPort3));
                    streamwriter.WriteLine(string.Format("LANPort4,{0}", GlobalData.logDetailResult.LANPort4));
                    streamwriter.WriteLine(string.Format("USB2.0,{0}", GlobalData.logDetailResult.USB2));
                    streamwriter.WriteLine(string.Format("USB3.0,{0}", GlobalData.logDetailResult.USB3));
                    streamwriter.WriteLine(string.Format("WPSButton,{0}", GlobalData.logDetailResult.WPSButton));
                    streamwriter.WriteLine(string.Format("ResetButton,{0}", GlobalData.logDetailResult.ResetButton));
                    streamwriter.WriteLine(string.Format("PowerLED,{0}", GlobalData.logDetailResult.PowerLED));
                    streamwriter.WriteLine(string.Format("PONLED,{0}", GlobalData.logDetailResult.PONLED));
                    streamwriter.WriteLine(string.Format("INETLED,{0}", GlobalData.logDetailResult.INETLED));
                    streamwriter.WriteLine(string.Format("WLANLED,{0}", GlobalData.logDetailResult.WLANLED));
                    streamwriter.WriteLine(string.Format("LAN1LED,{0}", GlobalData.logDetailResult.LAN1LED));
                    streamwriter.WriteLine(string.Format("LAN2LED,{0}", GlobalData.logDetailResult.LAN2LED));
                    streamwriter.WriteLine(string.Format("LAN3LED,{0}", GlobalData.logDetailResult.LAN3LED));
                    streamwriter.WriteLine(string.Format("LAN4LED,{0}", GlobalData.logDetailResult.LAN4LED));
                    streamwriter.WriteLine(string.Format("WPSLED,{0}", GlobalData.logDetailResult.WPSLED));
                    streamwriter.WriteLine(string.Format("LOSLED,{0}", GlobalData.logDetailResult.LOSLED));
                    streamwriter.WriteLine(string.Format("Error,{0}", GlobalData.logDetailResult.ERROR));
                    streamwriter.WriteLine(string.Format("TotalJudged,{0}", GlobalData.logDetailResult.JUDGED));
                    streamwriter.Dispose();
                    streamwriter.Close();
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
