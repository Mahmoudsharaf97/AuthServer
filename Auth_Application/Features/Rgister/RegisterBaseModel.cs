using Auth_Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Features
{
    public  class RegisterBaseModel
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }
        [JsonPropertyName("confirmPassword")]
        public string? ConfirmPassword { get; set; }
        [JsonPropertyName("birthDateMonth")]
        public int? BirthDateMonth { get; set; }
        [JsonPropertyName("birthDateYear")]
        public int? BirthDateYear { get; set; }
        [JsonPropertyName("registerType")]
        public byte? RegisterType { get; set; }
        [JsonProperty("captchaInput")]
        public string? CaptchaInput { get; set; }
        [JsonProperty("captchaToken")]
        public string? CaptchaToken { get; set; }
        [JsonProperty("nationalId")]
        public long? NationalId { get; set; }
        [JsonProperty("phone")]
        public long? Phone { get; set; }
        [JsonProperty("language")]
        public string? Language { get; set; }
        [JsonProperty("channel")]
        public string? Channel { get; set; }

        [JsonProperty("otp")]
        public int? OTP { get; set; }
        public virtual void ValidateRegisterModel()
        {
                if (string.IsNullOrEmpty(this.Email))
                    throw new AppException(ExceptionEnum.EmailExpired);
                if (RegisterType <1)
                    throw new AppException(ExceptionEnum.InvalidRegisterType);
                if (!this.NationalId.HasValue || NationalId <1)
                    throw new AppException(ExceptionEnum.NationalIdEmpty);
                if (!this.Phone.HasValue)
                    throw new AppException(ExceptionEnum.NationalIdEmpty);
        }
    }
}
