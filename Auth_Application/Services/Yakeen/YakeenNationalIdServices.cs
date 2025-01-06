using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core.Models;
using Auth_Core.UseCase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Auth_Application.Services
{
    public  class YakeenNationalIdServices : IYakeenNationalIdServices
    {
        private readonly IYakeenClient _yakeenClient;

        public YakeenNationalIdServices(IYakeenClient yakeenClient)
        {
            _yakeenClient = yakeenClient;
        }
        public  async Task<Tuple<UserDataModel, string, string>> GetUserDataFromYakeen(string nationalId, int birthYear, int birthMonth, string channel, string lang)
        {
           string logException = string.Empty;
           string outputDescription = string.Empty;
        //    outputDescription = WebResources.ResourceManager.GetString("ErrorGeneric", CultureInfo.GetCultureInfo(lang));
            UserDataModel userDataModel = null;

            try
            {
                long _nationalId = 0;
                long.TryParse(nationalId, out _nationalId);
                var customerYakeenRequest = new CustomerYakeenRequestDto()
                {
                    Nin = _nationalId,
                    IsCitizen = nationalId[0]=='1',
                    DateOfBirth = string.Format("{0}-{1}", birthMonth.ToString("00"), birthYear)
                };
                ServiceRequestLog predefinedlog = new ServiceRequestLog() { DriverNin = nationalId };
                var customerIdInfo = await _yakeenClient.GetCustomerIdInfo(customerYakeenRequest, predefinedlog);
                if (!customerIdInfo.Success)
                {
                    string ErrorMessage = //SubmitInquiryResource.ResourceManager.GetString($"YakeenError_{customerIdInfo.Error?.ErrorCode}", CultureInfo.GetCultureInfo(lang));
                  //  var GenericErrorMessage = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : SubmitInquiryResource.YakeenError_100;
                   // outputDescription = GenericErrorMessage;
                    logException = $"iYakeenClient.GetCustomerIdInfo return status {customerIdInfo.Success}, and error is: {JsonConvert.SerializeObject(customerIdInfo.Error?.ErrorMessage)}";
                    return Tuple.Create(userDataModel, logException, outputDescription);
                }

                userDataModel = new UserDataModel()
                {
                    IsExist = true,
                    FullNameAr = HandleUserName(customerIdInfo.FirstName, customerIdInfo.SecondName, customerIdInfo.ThirdName, customerIdInfo.LastName),
                    FullNameEn = HandleUserName(customerIdInfo.EnglishFirstName, customerIdInfo.EnglishSecondName, customerIdInfo.EnglishThirdName, customerIdInfo.EnglishLastName)
                };
                return Tuple.Create(userDataModel, logException, outputDescription);
            }
            catch (Exception ex)
            {
                logException = $"GetUserDataFromYakeen exception, and error is: {ex.ToString()}";
                return null;
            }
        }

        private string HandleUserName(string firstName, string secondName, string thirdName, string lastName)
        {
            var name = new List<string>
            {
                (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrEmpty(firstName)) ? "-" : firstName,
                (string.IsNullOrWhiteSpace(secondName) || string.IsNullOrEmpty(secondName)) ? "-" : secondName,
                (string.IsNullOrWhiteSpace(thirdName) || string.IsNullOrEmpty(thirdName)) ? "-" : thirdName,
                (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrEmpty(lastName)) ? "-" : lastName
            };

            return string.Join(" ", name);
        }
    }
}
