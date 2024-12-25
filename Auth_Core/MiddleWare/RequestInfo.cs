using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.MiddleWare
{
    public class RequestInfo
    {
        public string ServerIp { get; set; }
        public string UserIp { get; set; }
        public string UserAgent { get; set; }

      public void SetValues(string serverIp, string userIp, string userAgent)
      {
            this.ServerIp= serverIp;
            this.UserIp=userIp;
            this.UserAgent= userAgent;   
       }
    }
}
