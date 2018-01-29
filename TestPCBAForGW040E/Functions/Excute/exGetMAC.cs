using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040E.Functions {
    public class exGetMAC : baseFunctions {

        public bool getMACAddressOfDUT() {
            if (GlobalData.macAddress.Trim().Length != 0) return true;
            try {
                string _err;
                ContentGridFields t = new ContentGridFields() { INDEX="1", STEPNAME= "Login to DUT", STANDARD= "root login  on `console'", ACTUAL="?", RETRY="-", TIMEOUT="10000", JUDGED="?" };
                if (!login_toDUT(t, out _err)) return false;
                if (!read_MacAddress(t, out _err)) return false;
                return true;
            } catch {
                return false;
            }
        }
    }
}
