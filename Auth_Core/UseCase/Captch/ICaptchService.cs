
namespace Auth_Core.UseCase.Captch
{
    public  interface ICaptchService
    {
        Task<string> GenerateBase64Captcha(string captchaValue);
        public bool ValidateCaptchaToken(string captchToken, string captchInput, string Key);
    }
}
