using IdentityApplication.Interface;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using Auth_Core.Global;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using SME_Core;
using Auth_Core.Enums;
using Auth_Application.Services;
using Auth_Application.Services.Login;
using Auth_Application.Features;
using Auth_Application.Services.Captch;
using Auth_Core.UseCase.Captch;
using Azure.Core;
using static Auth_Application.Models.Errors;
using Auth_Application.Services.Registration;
namespace IdentityApplication.Services
{
    public partial class IdentityServices : IIdentityServices
    {
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IUsersCachedManager _usersCachedManager;
        private readonly AppSettingsConfiguration appSettingsConfiguration;
        private readonly GlobalInfo globalInfo;
        public UserManager<ApplicationUser<string>> _userManager { get; }
        public SignInManager<ApplicationUser<string>> _signInManager { get; }
        public ISessionServices _sessionServices { get; }
        //public Utilities _utilities  { get; }
        public   IRedisCaching _cacheManager { get; }
        public AppSettingsConfiguration settings { get; }
        public Utilities _utilities { get; }
        public ICaptchService _captchService { get; }

        public IdentityServices(IApplicationUserManager applicationUserManager, UserManager<ApplicationUser<string>> userManager
			, SignInManager<ApplicationUser<string>> signInManager, ISessionServices sessionServices, IRedisCaching cacheManager
		    , AppSettingsConfiguration _settings, GlobalInfo globalInfo, Utilities utilities
            , IUsersCachedManager usersCachedManager, ICaptchService captchService,AppSettingsConfiguration appSettingsConfiguration)
		{
			_applicationUserManager = applicationUserManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_sessionServices = sessionServices;
			//_utilities = utilities;
			_cacheManager = cacheManager;
			settings = _settings;
			this.globalInfo = globalInfo;
			_utilities = utilities;
			_usersCachedManager = usersCachedManager;
            _captchService = captchService;
            this.appSettingsConfiguration = appSettingsConfiguration;
        }
		public async Task<RegisterOutPut> Register(RegisterCommand model)
        {
            try
            {
                RegisterOutPut outPut = new RegisterOutPut();
                bool _captchValidat = _captchService.ValidateCaptchaToken(model.CaptchaToken, model.CaptchaInput, 
                    appSettingsConfiguration.CaptchKey);

                if (!_captchValidat)
                {
                    outPut.Succes =false;
                    outPut.errors.Add(new Error
                    {
                        ErrorCode = (int)ErrorCode.CaptchaError,
                        ErrorDescription = "Captch Invalid"
                    });
                    return outPut;
                }
                if (model.RegisterType==(int)RegisterType.Normal)
                {
                    RegistraionNormalStrategy registraionNormalStrategy = 
                        new RegistraionNormalStrategy(_cacheManager, _applicationUserManager,_userManager);

                   return await  registraionNormalStrategy.NormalRegistration(model);
                }
                else if(model.RegisterType == (int)RegisterType.VerifyYakeen)
                {

                }
            }
            catch (Exception ex)
            {
                var x = ex.ToString();
                throw;
            }
         
        }

        public async Task<bool> LogOut()
        {

            //var user = await _applicationUserManager.GetUserByIdAsync("globalInfo.CreatUser");
            var user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(LoginType.Email, "globalInfo.UserEmail");
            if (user == null)
                throw new AppException(ExceptionEnum.RecordNotExist);
             await _signInManager.SignOutAsync();
             await _cacheManager.DeleteSessionAsync(user.Id);
             return true;
        }



        //public async Task<bool> ResetPassword(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //        throw new AppException(ExceptionEnum.UserNotFound);

        //    var newSession = new SessionStatus
        //    {
        //        SessionId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15),
        //        UserId = user.Id,
        //        UserIP = _utilities.GetUserIP(),
        //        UserAgent = _utilities.GetUserAgent(),
        //        MacAddress = _utilities.GetMacAddress(),
        //        DeviceName = _utilities.GetUserIP()
        //    };
        //    await _sessionServices.AddSessionAsync(newSession);

        //    var token = await GetToken(user.Id, newSession.SessionId);
        //    if (token == null || token.ErrorCode != IdentityOutput.ErrorCodes.Success)
        //        throw new AppException(SME_Core.ExceptionEnum.GenricError);

        //    var Url = settings.MailSettings.IdentityUrlResetPassword + "&Id=" + token.Result?.Token;

        //    MailBodyModel mailbody = new MailBodyModel()
        //    {
        //        //   Lang= globalInfo.lang,
        //        SMSMethod = "",
        //    };
        //    string subject = "";//_mailService.CreateMailSubject(mailbody);
        //    string body = "";//await _mailService.CreateMailBody(mailbody);
        //    body = body?.Replace("#link#", Url);
        //    var res = true;//await _mailService.SendMailAsync("mahmoudsharaf97@gmail.com", body, subject);
        //    if (!res)
        //        throw new AppException(ExceptionEnum.SendMailFailed);
        //    return true;
        //}
        public async Task<bool> ConfirmResetPassword(ResetPasswordInput input)
        {
            var user = await _userManager.FindByIdAsync("");

            if (user == null)
                throw new AppException(ExceptionEnum.UserNotFound);

            var result = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!result.Succeeded)
            {
                if (result.Errors.ToList()[0].Code == "PasswordTooShort")
                    throw new AppException(ExceptionEnum.PasswordFormatNotValid);

                if (result.Errors.ToList()[0].Code == "PasswordRequiresUpper")
                    throw new AppException(ExceptionEnum.PasswordRequiresUpper);

                if (result.Errors.ToList()[0].Code == "PasswordRequiresNonAlphanumeric")
                    throw new AppException(ExceptionEnum.PasswordRequiresNonAlphanumeric);

                if (result.Errors.ToList()[0].Code == "PasswordRequiresDigit")
                    throw new AppException(ExceptionEnum.PasswordRequiresDigit);

                throw new AppException(ExceptionEnum.RecordUpdateFailed);
            }
           
            return true;
        }

        public Task<bool> ResetPassword(string email)
        {
            throw new NotImplementedException();
        }



        #region Login Begin & End 
        public async Task<LogInOutput> EndLogin(LogInInput model)
        {
            try
            {
                if (model.LoginType == (byte)LoginType.Email)
                {
                    EmailEndLoginStrategy emailLoginStrategy =
                        new EmailEndLoginStrategy(_signInManager, _cacheManager, _userManager, settings, _utilities, _usersCachedManager);
                    return await emailLoginStrategy.LoginByEmail(model.Email!, model.Password);
                }
                else if (model.LoginType == (byte)LoginType.NationalId)
                {
                    NationalIdEndLoginStrategy nationalIdLoginStrategy =
                        new NationalIdEndLoginStrategy(_signInManager, _cacheManager, _userManager, settings, _utilities, _usersCachedManager);
                    return await nationalIdLoginStrategy.LoginByNAtionalId(model.NationalId!.Value, model.Password);
                }
                return null;
            }
            catch (Exception ex)
            {
                var x = ex;
                throw new AppException(ExceptionEnum.UserLoginDataNotCorrect);
            }

        }
        public async Task<LogInOutput> BeginLogin(LogInInput model)
        {
            try
            {
                if (model.LoginType == (byte)LoginType.Email)
                {
                    EmailBeginLoginStrategy emailLoginStrategy =
                        new EmailBeginLoginStrategy(_signInManager, _cacheManager, _userManager, settings, _utilities,_usersCachedManager);
                    return await emailLoginStrategy.LoginByEmail(model.Email!, model.Password);
                }
                else if (model.LoginType == (byte)LoginType.NationalId)
                {
                    NationalIdBeginLoginStrategy nationalIdLoginStrategy =
                        new NationalIdBeginLoginStrategy(_signInManager, _cacheManager, _userManager, settings, _utilities, _usersCachedManager);
                    return await nationalIdLoginStrategy.LoginByNAtionalId(model.NationalId!.Value, model.Password);
                }
                return null;
            }
            catch (Exception ex)
            {
                var x = ex;
                throw new AppException(ExceptionEnum.UserLoginDataNotCorrect);
            }

        }

    
        #endregion
    }
}







