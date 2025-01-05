using Auth_Core.Global;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Auth_Core.Entity;
using MassTransit;
using Auth_Core.EventBus;
using Microsoft.Extensions.DependencyInjection;
namespace Auth_Core.MiddleWare
{
    public class LogMiddleWare 
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public LogMiddleWare(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task InvokeAsync(HttpContext context, GlobalInfo  globalInfo)
        {

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                //get QueryString
                var req = context?.Request;
                var QueryString = req?.QueryString.Value?.ToString();

                // get request body 
                string bodyStr;

             // req.EnableBuffering();
                //req.Body.Seek(0, SeekOrigin.Begin);
               // req.Body.Position = 0;
                using (StreamReader reader
                          = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }
                var model = DynamicToDictionary(JsonConvert.DeserializeObject(bodyStr));
                object[] args = new object[]  {
                     QueryString??""
                    , bodyStr ?? ""
                    , DateTime.Now
                    ,globalInfo?.EmailAddress
                    ,GetLocalIPAddress()
                    , globalInfo?.Channel
                    ,GetServerIp(context)
                    ,"" // Response time
                    ,error is AppException ?  ((int)((AppException)error)?.ErrorNumber).ToString() : error.Message
                    ,error is AppException ?  ((AppException)error)?.ErrorMessageEn : error.Message
                    ,error?.ToString()  // inner exception 
                };
                dynamic logEnitity = Activator.CreateInstance(typeof(IndentityLog), args);
                //   await MessageBroker.PublishMessageAsync(logEnitity, EntityName);

                ////
                /// the blew scope created as we cannot inject Scopped service inside any Middlewarw as middleware is injected as Singletone
                /// so we use IServiceScopeFactory to access the IEventBus inside the middleware
                /// 
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                    await eventBus.PublishAsync(logEnitity);
                }

                switch (error)
                {
                    case YakeenException ex:
                        {

                            break;
                        }
                    case AppException ex:
                        {
                            context.Response.Headers.Clear();
                            context.Response.ContentType = "text/plain";
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;

                            var obj = JsonConvert.SerializeObject(

                                    new BadRequestResult()
                                    {
                                        ErrorMsg = globalInfo.lang?.ToLower() == "en" ? ((AppException)error).ErrorMessageEn : ((AppException)error).ErrorMessageAr,
                                        ErrorNo = ((int)((AppException)error).ErrorNumber)
                                    }
                                );
                            await context.Response.WriteAsync(obj);
                            return;
                        }
                    default:
                        {
                            context.Response.Headers.Clear();
                            context.Response.ContentType = "text/plain";
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(globalInfo.lang?.ToLower() == "en" ? "Internal Server Error" : "حدث خطأ ما");
                            return;

                        }
                }
            }
        }


        public Dictionary<string, object> DynamicToDictionary(dynamic obj)
        {
            var dict = new Dictionary<string, object>();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(obj))
            {
                object obj2 = pd.GetValue(obj);
                dict.Add(pd.Name, obj2);
            }
            return dict;
        }
        public static string GetPropValue(Dictionary<string, object> myDict, string key)

        {
            string value = string.Empty;
            if (myDict.ContainsKey(key))
            {
                value = myDict[key].ToString();
            }
            return value;
        }


        public static string GetServerIp(HttpContext context)
        {
            try
            {
                IPAddress ipAddressString = context.Connection.LocalIpAddress;
                string REMOTE_ADDR = context.Connection.RemoteIpAddress.ToString();
                string LOCAL_ADDR = context.Connection.LocalIpAddress.ToString();
                string SERVER_ADDR = context.Connection.LocalIpAddress.ToString();
                string REMOTE_HOST = context.Connection.RemoteIpAddress.ToString();

                string result = "LocalIpAddress: " + ipAddressString.ToString();
                result += "  REMOTE_ADDR: " + REMOTE_ADDR + "  LOCAL_ADDR:" + LOCAL_ADDR
                    + "  SERVER_ADDR:" + SERVER_ADDR + "  REMOTE_HOST:" + REMOTE_HOST;

                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {

                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string GetUserExternalIp(HttpContext context)
        {
            try
            {
                IPAddress remoteIpAddress = context.Connection.RemoteIpAddress;
                string result = "";
                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .First(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }
                    result = remoteIpAddress.ToString();
                }
                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public class BadRequestResult
        {
            public string ErrorMsg { get; set; }
            public int ErrorNo { get; set; }

        }
    }
}
