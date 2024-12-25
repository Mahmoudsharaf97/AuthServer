using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.MiddleWare
{
    public class RequestMiddleWare
    {
        private readonly RequestDelegate _next;
        public RequestMiddleWare(RequestDelegate next)
        {
                _next = next;
        }
        public async  Task InvokeAsync(HttpContext context)
        {
            var userIp = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var serverIp = GetServerIpAddress();
            await _next(context);
        }
        private string GetServerIpAddress()
        {
            // This retrieves the IP address of the local machine
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                {
                    return ip.ToString();
                }
            }

            return "No IP found";
        }
    }
}
}
