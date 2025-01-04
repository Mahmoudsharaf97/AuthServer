using IdentityApplication.Models;
using MediatR;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
namespace Auth_Application.Features
{
    public class RegisterCommand : IRequest<RegisterOutPut>
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
    }
}
