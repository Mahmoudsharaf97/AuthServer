using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.Models;
using Auth_Core.Models.Yakeen;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Infrastructure.Yakeen.Driver
{
    public  class YakeenClient
    {
        private readonly AppSettingsConfiguration _appSettings;

        public YakeenClient(AppSettingsConfiguration appSettings)
        {
            _appSettings = appSettings;
        }
        #region IntegrationDriver 
        private async Task<YakeenOutput> NonSaudiByIqama(YakeenRequestDto requset, ServiceRequestLog log)
        {
            YakeenOutput output = new YakeenOutput();
            DateTime dateBeforeCalling = DateTime.Now;
            NonSaudiByIqamaDto yakeenDto = new NonSaudiByIqamaDto();
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                if (requset == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullRequest.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                   // ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }
                if (requset.nonSaudiDto == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullRequest.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                  //  ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }
                if (string.IsNullOrEmpty(requset.nonSaudiDto.iqama))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.SequenceNumberIsNullOrEmpty;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.SequenceNumberIsNullOrEmpty.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
         //           ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }

                if (string.IsNullOrEmpty(requset.nonSaudiDto.birthDateG))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ModelYearIsNull;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.ModelYearIsNull.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
               //     ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }

                string stringPayload = string.Empty;


                //#region TestOnlyRemoveLater

                //requset.nonSaudiDto.iqama = "2130635150";
                //requset.nonSaudiDto.birthDateG = "1978-12";
                //#endregion

                requset.nonSaudiDto.Channel = "Motor";
                requset.nonSaudiDto.lang = "ar";
                stringPayload = JsonConvert.SerializeObject(requset.nonSaudiDto);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(1);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", svcCredentials);

                string apiName = _appSettings.YakeenLocalURl + "GetNonSaudiByIqama";
                log.ServiceURL = apiName;
                var result = await client.PostAsync(apiName, httpContent);

                response = result;

                if (response == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "Yakeen response is" + YakeenOutput.ErrorCodes.NullResponse.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                   // ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }

                if (!response.IsSuccessStatusCode)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                    output.ErrorDescription = $"Yakeen failed with status code: {response.StatusCode}";

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                   // ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }

                if (response.Content == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullResponse.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "Yakeen Respone Content is null";
                   // ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }
                if (string.IsNullOrEmpty(await response.Content.ReadAsStringAsync()))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullResponse.ToString();
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "Yakeen response content result return null";
                 //   ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync());
                yakeenDto = JsonConvert.DeserializeObject<NonSaudiByIqamaDto>(await response.Content.ReadAsStringAsync());

                if (yakeenDto == null) // add list of error and check it done 
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.ServiceError.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "failure in convert thr response to yakeenDto";
                //    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;

                }
                if (yakeenDto.errorDetail != null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ServiceError;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.ServiceError.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "errorId: " + yakeenDto.errorId + "\n" + JsonConvert.SerializeObject(yakeenDto);
               //     ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }

                yakeenDto.gender = ConvertGenderToEGender(yakeenDto.Egender);
                output.NonSaudiByIqamaDto = yakeenDto;
                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = YakeenOutput.ErrorCodes.Success.ToString();

                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceResponseTimeInSeconds = (DateTime.Now - dateBeforeCalling).TotalSeconds;
             //   ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                return output;

            }
            catch (Exception ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceError;
                output.ErrorDescription = YakeenOutput.ErrorCodes.ServiceError.ToString();

                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = ex.ToString();
                log.ServiceResponseTimeInSeconds = (DateTime.Now - dateBeforeCalling).TotalSeconds;
              //  ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                throw ex;
            }
        }
        private async Task<YakeenOutput> SaudiByNin(YakeenRequestDto requset, ServiceRequestLog log)
        {
            YakeenOutput output = new YakeenOutput();
            DateTime dateBeforeCalling = DateTime.Now;
            SaudiByNinDto yakeenDto = new SaudiByNinDto();
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                if (requset == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullRequest.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                  //  ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }
                if (requset.saudiDto == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullRequest.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                  //  ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }

                if (string.IsNullOrEmpty(requset.saudiDto.nin))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "saudiDto.nin is null";

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                  //  ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }

                if (string.IsNullOrEmpty(requset.saudiDto.dateString))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.DateOfBirthEmpty;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.DateOfBirthEmpty.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                 //   ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }


                requset.saudiDto.Channel = "Motor";
                requset.saudiDto.lang = "ar";

                string stringPayload = string.Empty;
                stringPayload = JsonConvert.SerializeObject(requset.saudiDto);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(20);

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJCQ0FSRV9UUklBTF9ZQUsiLCJlbnZpcm9ubWVudCI6IlRSSUFMIiwicm9sZXMiOlsiQ0xJRU5UIl0sImlzcyI6IkVsbSIsImV4cCI6MTcwMjk5MjQ2MCwiY2xpZW50LWlkIjoxMTYsImlhdCI6MTcwMjkwNjA2MH0.Y-oCvXd4-17ZbCmpPuQ3gYx0D3npHrfzOBjIgnaa4QU");
                //client.DefaultRequestHeaders.Add("Accept-Language", string.IsNullOrEmpty(requset.lang) ? "ar" : requset.lang);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _appSettings.YakeenLocalsvcCredentials);
                string apiName = _appSettings.YakeenLocalURl + "GetSaudiByNin";
                var result = await client.PostAsync(apiName, httpContent);
                log.ServiceURL = apiName;


                response = result;

                if (response == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "Yakeen response is" + YakeenOutput.ErrorCodes.NullResponse.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                  //  ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }

                if (!response.IsSuccessStatusCode)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                    output.ErrorDescription = $"Yakeen failed with status code: {response.StatusCode}";

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    return output;
                }

                if (response.Content == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullResponse.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "Yakeen Respone Content is null";
                //    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    return output;
                }
                if (string.IsNullOrEmpty(await response.Content.ReadAsStringAsync()))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.NullResponse.ToString();
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "Yakeen response content result return null";
                //    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }

                log.ServiceResponse = JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync());
                yakeenDto = JsonConvert.DeserializeObject<SaudiByNinDto>(await response.Content.ReadAsStringAsync());



                if (yakeenDto == null) // add list of error and check it done 
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.ServiceError.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "failure in convert thr response to yakeenDto";
             //       ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;

                }
                if (yakeenDto.errorDetail != null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ServiceError;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.ServiceError.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = "errorId: " + yakeenDto.errorId + "\n" + JsonConvert.SerializeObject(yakeenDto);
             //       ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }

                yakeenDto.gender = ConvertGenderToEGender(yakeenDto.Egender);
                output.SaudiByNinDto = yakeenDto;
                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = YakeenOutput.ErrorCodes.Success.ToString();

                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceResponseTimeInSeconds = (DateTime.Now - dateBeforeCalling).TotalSeconds;
           //     ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                return output;

            }
            catch (Exception ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceError;
                output.ErrorDescription = YakeenOutput.ErrorCodes.ServiceError.ToString();

                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = ex.ToString();
                log.ServiceResponseTimeInSeconds = (DateTime.Now - dateBeforeCalling).TotalSeconds;
        //        ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                return output;
            }
        }
        #endregion

        private Gender ConvertGenderToEGender(string gender)
        {
            Gender eGender = Gender.U;
            if (!string.IsNullOrEmpty(gender))
            {
                if (!Enum.TryParse(gender, out eGender))
                {
                    eGender = Gender.U;
                }
            }


            return eGender;
        }
    }
}
