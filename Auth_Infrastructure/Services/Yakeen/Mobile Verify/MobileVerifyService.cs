using Auth_Core;
using Auth_Core.Models;
using Auth_Core.Models.Yakeen;
using Auth_Core.UseCase.Yakeen;
using Newtonsoft.Json;
using SME_Core;
namespace Auth_Infrastructure.Yakeen
{
    public class MobileVerifyService : IMobileVerifyService
    {
        private readonly AppSettingsConfiguration _appSettings;

        public MobileVerifyService(AppSettingsConfiguration appSettings)
        {
            _appSettings = appSettings;
        }
        public async Task<YakeenMobileVerificationOutput> YakeenMobileVerificationAsync(long phone,long nationalId, string Language)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenMobileVerificationOutput output = new YakeenMobileVerificationOutput();
            try
            {
                if (phone <1 ||nationalId<1)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "request is null";
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                log.DriverNin = nationalId.ToString();
                log.Phone = phone.ToString();
                log.Method = "Yakeen-getYakeenMobileVerification";
                //  string Url = "https://yakeen-lite.api.elm.sa/api/v1/person/@id/owns-mobile/@mobilenumber";
                string Url = _appSettings.YakeenMobileVerifyUrl;
                log.ServiceURL = Url;
                string serviceRequest = Url.Replace("@mobilenumber", Utilities.ValidatePhoneNumber(log.Phone));
                serviceRequest = serviceRequest.Replace("@id", log.DriverNin);
                log.ServiceRequest = serviceRequest;
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(_appSettings.YakeenMobileVerifyTimeOutInsecond);
                //httpClient.DefaultRequestHeaders.Add("APP-ID", "a56a2495");
                //httpClient.DefaultRequestHeaders.Add("APP-KEY", "33bfd1bf76a0502f9ec2380dd7d53012");
                //httpClient.DefaultRequestHeaders.Add("SERVICE_KEY", "265532d5-6330-49b0-b784-e5ea01c9b678");
                //httpClient.DefaultRequestHeaders.Add("ORGANIZATION-NUMBER", "7001903785");
                httpClient.DefaultRequestHeaders.Add("APP-ID", _appSettings.YakeenMobileVerifyAPPID);
                httpClient.DefaultRequestHeaders.Add("APP-KEY", _appSettings.YakeenMobileVerifyAPPKEY);
                httpClient.DefaultRequestHeaders.Add("SERVICE_KEY", _appSettings.YakeenMobileVerifySERVICEKEY);
                httpClient.DefaultRequestHeaders.Add("ORGANIZATION-NUMBER", _appSettings.YakeenMobileVerifyORGANIZATIONNUMBER);
                var YakeenResponse = await httpClient.GetAsync(serviceRequest);
                string response = await YakeenResponse.Content.ReadAsStringAsync();

                DateTime dtBeforeCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ServiceResponse = response;
                if (string.IsNullOrEmpty(response))
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "yakeenMobileVerificationResponse is null";
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                dynamic responseAfterDeserialize = JsonConvert.DeserializeObject(response)!;
                MobileVerificationModel responseObject = JsonConvert.DeserializeObject<MobileVerificationModel>(responseAfterDeserialize.ToString());
                if (responseObject == null)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.ServiceError;
                    output.ErrorDescription = "responseObject is null";
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                output.mobileVerificationModel = responseObject;
                int responseCode = 0;
                int.TryParse(responseObject?.code.ToString(), out responseCode);
                if (responseCode == 100)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.InternalError;
                    output.ErrorDescription = responseObject.message;
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                if (responseCode == 101)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.InvalidId;
                    output.ErrorDescription = responseObject.message;
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                if (responseCode == 102)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.InvalidMobileNumber;
                    output.ErrorDescription = responseObject.message;
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                if (responseCode == 103)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.InvalidServiceKey;
                    output.ErrorDescription = responseObject.message;
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                if (!responseObject.isOwner)
                {
                    output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.InvalidMobileOwner;
                    output.ErrorDescription = "Phone is Not belong to national id";
                    AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                    return output;
                }
                output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                return output;
            }
   
            catch (Exception ex)
            {
                output.ErrorCode = YakeenMobileVerificationOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.ToString();
                AddToYakeenLog(log, output.ErrorCode, output.ErrorDescription, output.ErrorDescription, (int)output.ErrorCode);
                return output;
            }

        }

 

        private void AddToYakeenLog(ServiceRequestLog log, YakeenMobileVerificationOutput.ErrorCodes errorCodes, string ServiceErrorDescription, string errorDescription, int errorCode)
        {
            log.ServiceErrorCode = errorCodes.ToString();
            log.ServiceErrorDescription = ServiceErrorDescription;
            log.ErrorCode = errorCode;
            log.ErrorDescription = errorDescription;
            // ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
        }
    }
}
