using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.Models;
using Auth_Core.Models.Yakeen;
using Auth_Core.UseCase;
using Newtonsoft.Json;
using SME_Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Infrastructure.Yakeen
{
    public  class YakeenClient : IYakeenClient
    {
        private readonly AppSettingsConfiguration _appSettings;

        public YakeenClient(AppSettingsConfiguration appSettings)
        {
            _appSettings = appSettings;
        }
        #region IntegrationDriver 

        public async Task<CustomerIdYakeenInfoDto> GetCustomerIdInfo(CustomerYakeenRequestDto request, ServiceRequestLog predefinedLogInfo)
        {
            try
            {
                if (request.IsCitizen)
                    return await GetCitizenIdInfo(request, predefinedLogInfo);
                else
                    return await GetAlienInfoByIqama(request, predefinedLogInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<CustomerIdYakeenInfoDto> GetCitizenIdInfo(CustomerYakeenRequestDto request, ServiceRequestLog log)
        {
            CustomerIdYakeenInfoDto customerIdYakeenInfoDto = new CustomerIdYakeenInfoDto();
            customerIdYakeenInfoDto.Success = false;
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Nin.ToString()) || string.IsNullOrEmpty(request.DateOfBirth))
                {
                    customerIdYakeenInfoDto.Success = false;
                    customerIdYakeenInfoDto.Error.Type = EErrorType.LocalError;
                    customerIdYakeenInfoDto.Error.ErrorMessage = "Yakeen-getCitizenIDInfo request is null";
                    customerIdYakeenInfoDto.Error.ErrorCode = YakeenOutput.ErrorCodes.NullRequest.ToString();
                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.NullRequest;
                    log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    return customerIdYakeenInfoDto;
                }
                YakeenRequestDto requestDto = new YakeenRequestDto();
                var saudiDto = new SaudiDto();
                saudiDto.nin = request.Nin.ToString();
                saudiDto.dateString = request.DateOfBirth;
                requestDto.saudiDto = saudiDto;
                log.ServiceRequest = JsonConvert.SerializeObject(saudiDto);
                DateTime dtBeforeCalling = DateTime.Now;
                var yakeenResult = await SaudiByNin(requestDto, log);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;
                if (yakeenResult == null)
                {
                    customerIdYakeenInfoDto.Success = false;
                    customerIdYakeenInfoDto.Error.Type = EErrorType.YakeenError;
                    customerIdYakeenInfoDto.Error.ErrorMessage = "SaudiByNin response return null";
                    customerIdYakeenInfoDto.Error.ErrorCode = YakeenOutput.ErrorCodes.NullResponse.ToString();
                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.NullResponse;
                    log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    return customerIdYakeenInfoDto;
                }
                if (yakeenResult.SaudiByNinDto == null)
                {
                    customerIdYakeenInfoDto.Success = false;
                    customerIdYakeenInfoDto.Error.Type = EErrorType.YakeenError;
                    customerIdYakeenInfoDto.Error.ErrorMessage = "SaudiByNin response return null";
                    customerIdYakeenInfoDto.Error.ErrorCode = YakeenOutput.ErrorCodes.NullResponse.ToString();
                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.NullResponse;
                    log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    return customerIdYakeenInfoDto;
                }

                customerIdYakeenInfoDto.Success = true;
                customerIdYakeenInfoDto.IsCitizen = true;
                customerIdYakeenInfoDto.FirstName = yakeenResult.SaudiByNinDto.firstName;
                customerIdYakeenInfoDto.SecondName = yakeenResult.SaudiByNinDto.fatherName;
                customerIdYakeenInfoDto.ThirdName = yakeenResult.SaudiByNinDto.grandFatherName;
                customerIdYakeenInfoDto.LastName = yakeenResult.SaudiByNinDto.familyName;
                customerIdYakeenInfoDto.EnglishFirstName = yakeenResult.SaudiByNinDto.englishFirstName;
                customerIdYakeenInfoDto.EnglishSecondName = yakeenResult.SaudiByNinDto.englishSecondName;
                customerIdYakeenInfoDto.EnglishThirdName = yakeenResult.SaudiByNinDto.englishThirdName;
                customerIdYakeenInfoDto.EnglishLastName = yakeenResult.SaudiByNinDto.englishLastName;
                customerIdYakeenInfoDto.SubtribeName = yakeenResult.SaudiByNinDto.subtribeName;
                customerIdYakeenInfoDto.Gender = yakeenResult.SaudiByNinDto.gender;
                customerIdYakeenInfoDto.LogId = yakeenResult.SaudiByNinDto.logId;
                if (!string.IsNullOrEmpty(yakeenResult.SaudiByNinDto.dateOfBirthG))
                {
                    customerIdYakeenInfoDto.DateOfBirthG = DateTime.ParseExact(yakeenResult.SaudiByNinDto.dateOfBirthG, "dd-MM-yyyy", new CultureInfo("en-US"));
                }
                else
                {
                    DateTime dateOfBirth = new DateTime(1, 1, 1);
                    if (DateTime.TryParse("01-" + request.DateOfBirth, out dateOfBirth))
                    {
                        customerIdYakeenInfoDto.DateOfBirthG = dateOfBirth;
                    }
                }
                customerIdYakeenInfoDto.DateOfBirthH = yakeenResult.SaudiByNinDto.dateOfBirthH;
                customerIdYakeenInfoDto.IdExpiryDate = yakeenResult.SaudiByNinDto.idExpiryDate;
                customerIdYakeenInfoDto.SocialStatus = yakeenResult.SaudiByNinDto.socialStatusDetailedDesc;
                customerIdYakeenInfoDto.OccupationCode = yakeenResult.SaudiByNinDto.occupationCode;
                customerIdYakeenInfoDto.Error.ErrorMessage = YakeenOutput.ErrorCodes.Success.ToString();
                customerIdYakeenInfoDto.Error.ErrorCode = ((int)YakeenOutput.ErrorCodes.Success).ToString();
                log.ErrorCode = (int)YakeenOutput.ErrorCodes.Success;
                log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                log.ServiceErrorCode = YakeenOutput.ErrorCodes.Success.ToString();
                log.ServiceErrorDescription = log.ErrorDescription;
                return customerIdYakeenInfoDto;
            }
            catch (Exception ex)
            {
                customerIdYakeenInfoDto.Error.Type = EErrorType.LocalError;
                customerIdYakeenInfoDto.Error.ErrorMessage = ex.ToString();
                customerIdYakeenInfoDto.Error.ErrorCode = ((int)YakeenOutput.ErrorCodes.ServiceException).ToString();
                log.ErrorCode = int.Parse(customerIdYakeenInfoDto.Error.ErrorCode);
                log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                throw new Exception(ex.ToString());
            }

        }
        private async Task<CustomerIdYakeenInfoDto> GetAlienInfoByIqama(CustomerYakeenRequestDto request, ServiceRequestLog log)
        {
            CustomerIdYakeenInfoDto customerIdYakeenInfoDto = new CustomerIdYakeenInfoDto();
            YakeenRequestDto requestDto = new YakeenRequestDto();
            log.Channel = "Portal";
            log.ServiceRequest = JsonConvert.SerializeObject(request);
            log.Method = "Yakeen-getAlienInfoByIqama";
            //log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Nin.ToString()) || string.IsNullOrEmpty(request.DateOfBirth))
                {

                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.NullRequest;
                    log.ErrorDescription = "GetAlienInfoByIqama request is null";
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    customerIdYakeenInfoDto.Success = false;
                    customerIdYakeenInfoDto.Error.Type = EErrorType.LocalError;
                    customerIdYakeenInfoDto.Error.ErrorMessage = "GetAlienInfoByIqama request is null";
                    return customerIdYakeenInfoDto;
                }
                requestDto.nonSaudiDto = new NonSaudiDto();

                requestDto.nonSaudiDto.iqama = request.Nin.ToString();
                requestDto.nonSaudiDto.birthDateG = request.DateOfBirth;

                log.ServiceRequest = JsonConvert.SerializeObject(requestDto);
                DateTime dtBeforeCalling = DateTime.Now;

                var yakeenResult = await NonSaudiByIqama(requestDto, log);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (yakeenResult == null)
                {
                    customerIdYakeenInfoDto.Error.Type = EErrorType.LocalError;
                    customerIdYakeenInfoDto.Error.ErrorMessage = "response return null";
                    customerIdYakeenInfoDto.Error.ErrorCode = YakeenOutput.ErrorCodes.NullResponse.ToString();
                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.NullResponse;
                    log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    return customerIdYakeenInfoDto;
                }
                if (yakeenResult.NonSaudiByIqamaDto == null)
                {
                    customerIdYakeenInfoDto.Success = false;

                    customerIdYakeenInfoDto.Error.Type = EErrorType.YakeenError;
                    customerIdYakeenInfoDto.Error.ErrorMessage = "NonSaudiByIqama response return null";
                    customerIdYakeenInfoDto.Error.ErrorCode = YakeenOutput.ErrorCodes.NullResponse.ToString();
                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.NullResponse;
                    log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    return customerIdYakeenInfoDto;
                }
                if (string.IsNullOrEmpty(yakeenResult.NonSaudiByIqamaDto.dateOfBirthG))
                {
                    customerIdYakeenInfoDto.Success = false;
                    customerIdYakeenInfoDto.Error.Type = EErrorType.YakeenError;
                    customerIdYakeenInfoDto.Error.ErrorCode = "16";
                    customerIdYakeenInfoDto.Error.ErrorMessage = "(AlienInfoByIqama) returned DateOfBirthG empty";

                    log.ErrorCode = (int)YakeenOutput.ErrorCodes.DateOfBirthGIsEmpty;
                    log.ErrorDescription = customerIdYakeenInfoDto.Error.ErrorMessage;
                    return customerIdYakeenInfoDto;
                }


                customerIdYakeenInfoDto.Success = true;
                customerIdYakeenInfoDto.IsCitizen = false;

                customerIdYakeenInfoDto.FirstName = yakeenResult.NonSaudiByIqamaDto.firstName;
                customerIdYakeenInfoDto.SecondName = yakeenResult.NonSaudiByIqamaDto.secondName;
                customerIdYakeenInfoDto.ThirdName = yakeenResult.NonSaudiByIqamaDto.thirdName;
                customerIdYakeenInfoDto.LastName = yakeenResult.NonSaudiByIqamaDto.lastName;
                customerIdYakeenInfoDto.EnglishFirstName = yakeenResult.NonSaudiByIqamaDto.englishFirstName;
                customerIdYakeenInfoDto.EnglishSecondName = yakeenResult.NonSaudiByIqamaDto.englishSecondName;
                customerIdYakeenInfoDto.EnglishThirdName = yakeenResult.NonSaudiByIqamaDto.englishThirdName;
                customerIdYakeenInfoDto.EnglishLastName = yakeenResult.NonSaudiByIqamaDto.englishLastName;

                customerIdYakeenInfoDto.NationalityCode = yakeenResult.NonSaudiByIqamaDto.nationalityCode;
                customerIdYakeenInfoDto.Gender = yakeenResult.NonSaudiByIqamaDto.gender;
                customerIdYakeenInfoDto.LogId = yakeenResult.NonSaudiByIqamaDto.logId;
                customerIdYakeenInfoDto.DateOfBirthG = DateTime.ParseExact(yakeenResult.NonSaudiByIqamaDto.dateOfBirthG, "dd-MM-yyyy", new CultureInfo("en-US"));
                customerIdYakeenInfoDto.DateOfBirthH = yakeenResult.NonSaudiByIqamaDto.dateOfBirthH;
                customerIdYakeenInfoDto.IdExpiryDate = yakeenResult.NonSaudiByIqamaDto.iqamaExpiryDateH;
                customerIdYakeenInfoDto.SocialStatus = yakeenResult.NonSaudiByIqamaDto.socialStatus;
                customerIdYakeenInfoDto.OccupationDesc = yakeenResult.NonSaudiByIqamaDto.occupationDesc;
                log.ErrorCode = (int)YakeenOutput.ErrorCodes.Success;
                log.ErrorDescription = YakeenOutput.ErrorCodes.Success.ToString();
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ErrorDescription;
                return customerIdYakeenInfoDto;
            }
            catch (Exception ex)
            {
                log.ErrorCode = (int)YakeenOutput.ErrorCodes.ServiceException;
                log.ErrorDescription = ex.ToString();
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                throw ex;
            }
        }

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
                    return output;
                }
                if (string.IsNullOrEmpty(requset.nonSaudiDto.iqama))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.SequenceNumberIsNullOrEmpty;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.SequenceNumberIsNullOrEmpty.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    return output;
                }

                if (string.IsNullOrEmpty(requset.nonSaudiDto.birthDateG))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.ModelYearIsNull;
                    output.ErrorDescription = YakeenOutput.ErrorCodes.ModelYearIsNull.ToString();

                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    return output;
                }

                string stringPayload = string.Empty;

                requset.nonSaudiDto.Channel = "Motor";
                requset.nonSaudiDto.lang = "ar";
                stringPayload = JsonConvert.SerializeObject(requset.nonSaudiDto);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(1);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _appSettings.YakeenLocalsvcCredentials);

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
        #region Phone Registration
        public async Task<YakeenMobileVerificationOutput> YakeenMobileVerificationAsync(long phone, long nationalId, string Language)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenMobileVerificationOutput output = new YakeenMobileVerificationOutput();
            try
            {
                if (phone < 1 || nationalId < 1)
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
