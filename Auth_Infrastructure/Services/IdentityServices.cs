//using System.Text;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using Auth_Core.Global;
//using Auth_Core;
//using Auth_Core.UseCase.Redis;
//using Auth_Infrastructure.Identity;
//using Microsoft.AspNetCore.Identity;

//namespace IdentityApplication.Services
//{
//    public class IdentityServices : IIdentityServices
//    {
//        private readonly GlobalInfo globalInfo;

//        public ApplicationUserManager _userManager { get; }
//        public SignInManager<ApplicationUser<string>> _signInManager { get; }
//        public ISessionServices _sessionServices { get; }
//      //  public Utilities _utilities  { get; }
//        public   IRedisCaching _cacheManager { get; }
//        public AppSettingsConfiguration settings { get; }
//        public IdentityServices(ApplicationUserManager userManager ,SignInManager<ApplicationUser<string>> signInManager,ISessionServices sessionServices , Utilities utilities, IRedisCaching cacheManager
//          ,AppSettingsConfiguration _settings,GlobalInfo globalInfo
//            )
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _sessionServices = sessionServices;
//            _utilities = utilities;
//            _cacheManager = cacheManager;
//            settings= _settings;
//            this.globalInfo = globalInfo;
//        }
//        public async Task<RegisterOutPut> Register(RegisterInput model)
//        {
//            try
//            {
//                var result = new RegisterOutPut();
//                var emailExist = await _userManager.CheckEmailExistAsync(model.Email);

//                if (emailExist)
//                    throw new AppException(SME_Core.ExceptionEnum.EmailAlreadyExist);

//                // var user = MapperObject.Mapper.Map<ApplicationUser<string>>(model);
//                var user = new ApplicationUser<string>
//                {
//                    Email = model.Email,
//                    PasswordHash = model.Password,
//                    UserName = model.Email,
//                    PhoneNumberConfirmed = false,
//                    EmailConfirmed = false,
//                };

//                var createdUser = await _userManager.CreateAsync(user, model.Password);
//                if (!createdUser.Succeeded)
//                {
//                    if (createdUser.Errors.ToList()[0].Code == "InvalidUserName")
//                        throw new AppException(SME_Core.ExceptionEnum.InvalidUserName);

//                    if (createdUser.Errors.ToList()[0].Code == "InvalidEmail")
//                        throw new AppException(SME_Core.ExceptionEnum.InvalidUserName);

//                    if (createdUser.Errors.ToList()[0].Code == "PasswordTooShort")
//                        throw new AppException(SME_Core.ExceptionEnum.PasswordFormatNotValid);

//                    if (createdUser.Errors.ToList()[0].Code == "PasswordRequiresUpper")
//                        throw new AppException(SME_Core.ExceptionEnum.PasswordRequiresUpper);

//                    if (createdUser.Errors.ToList()[0].Code == "PasswordRequiresNonAlphanumeric")
//                        throw new AppException(SME_Core.ExceptionEnum.PasswordRequiresNonAlphanumeric);

//                    if (createdUser.Errors.ToList()[0].Code == "PasswordRequiresDigit")
//                        throw new AppException.ExceptionEnum.PasswordRequiresDigit);

//                    throw new AppException(SME_Core.ExceptionEnum.RecordUpdateFailed);
//                }
//                result.Succes = true;
//                return result;
//            }
//            catch (Exception ex)
//            {
//                var x = ex.ToString();
//                throw;
//            }
         
//        }

//        public async Task<LogInOutput> Login(LogInInput model)
//        {
//            try
//            {
//                var output = new LogInOutput();
//                var user = await _userManager.GetUserByEmailAsync(model.Email.Trim());

//                if (user == null)
//                    throw new AppException(SME_Core.ExceptionEnum.UserNotFound);

//                if (user.IsDeleted)
//                    throw new AppException(SME_Core.ExceptionEnum.UserDeleted);

//                //if (!user.PhoneNumberConfirmed)
//                //    throw new AppException(ExceptionEnum.UserPhoneNotActiveConfirmed);

//                //if (!user.EmailConfirmed)
//                //    throw new AppException(ExceptionEnum.UserEmailNotconfirmed);

//                if (user.LockoutEnd != null)
//                    throw new AppException(SME_Core.ExceptionEnum.UserIsLocked);
                
//                var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, false, true); 
//                if (result.Succeeded)
//                {
//                    var activeSessions = _sessionServices.GetUsrerAllSessions(user.Id);
//                    if (activeSessions.Count > 0)
//                    {
//                        foreach (var item in activeSessions)
//                        {
//                            await _sessionServices.Removeasync(item);
//                            await _cacheManager.RemoveAsync(item.SessionId);
//                        }

//                    }
//                    var newSession = new SessionStatus
//                    {
//                        SessionId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15),
//                        UserId = user.Id,
//                        UserIP = _utilities.GetUserIP(),
//                        UserAgent = _utilities.GetUserAgent(),
//                        MacAddress = _utilities.GetMacAddress(),
//                        DeviceName = _utilities.GetUserIP()
//                    };
//                    await _sessionServices.AddSessionAsync(newSession);
//                    user.LastSuccessLogin = DateTime.Now;
//                    await _userManager.UpdateAsync(user);



//                    var tokenResult =await GetToken(user.Id, newSession.SessionId);
//                    //  var tokenResult =await CreateJwtToken(user , newSession.SessionId);
//                    if (tokenResult.ErrorCode == IdentityOutput.ErrorCodes.Success)
//                    output.Token = tokenResult?.Result.Token;
//                    output.success = true;
//                    output.Token = tokenResult.Result.Token;
//                    //output.Token = new JwtSecurityTokenHandler().WriteToken(tokenResult);
//                    return output;
//                }
//                return null;
//            }
//            catch (Exception ex)
//            {
//                var x = ex;
//                throw new AppException(SME_Core.ExceptionEnum.UserLoginDataNotCorrect);
//            }

//        }


//        public string GenerateTokenJWT(string ID, string Email, string userName, string sessionId)

//        {


//            var claims = new[]

//                       {
//                          new Claim(ClaimTypes.NameIdentifier, ID),

//                          new Claim(ClaimTypes.Email, Email),

//                          new Claim(ClaimTypes.Name, sessionId),

//                          new Claim(ClaimTypes.PrimarySid, "123"),

//                          new Claim(ClaimTypes.SerialNumber, sessionId),

//                          new Claim("auth_time", DateTime.Now.ToString()),
//                        };



//            var secrectkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT.SecretKey));

//            var creds = new SigningCredentials(secrectkey, SecurityAlgorithms.HmacSha256);



//            JwtSecurityToken token = new JwtSecurityToken("Bcare",

//              "Bcare",

//              claims,

//              expires: DateTime.Now.AddMinutes(settings.ExpirySettings.TokenExpiryMinutes), 

//              signingCredentials: creds);

//          //  key = x.ToString();

//            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

//            return encodedJwt;

//        }
//        public async Task<IdentityOutput> GetToken(string userId, string SessionId)
//        {
//            var output = new IdentityOutput
//            {
//                Result = new LogInOutput()
//            };

//            var user = await _userManager.FindByIdAsync(userId);

//            if (user == null)
//                throw new AppException(SME_Core.ExceptionEnum.RecordNotExist);




//            var tokenDescriptor = new SecurityTokenDescriptor
//            {

//                Subject = new ClaimsIdentity(new Claim[]
//                          {
//                        new Claim("Any","No"),
//                        new Claim("UserId",user.Id.ToString()),
//                        new Claim("SessionId",SessionId),
//                        new Claim("Email",user.Email.ToString()),
//                        new Claim("LockoutEnabled",user.LockoutEnabled.ToString()),
//                        new Claim("LockoutEnd",user.LockoutEnd.ToString()),
//                        new Claim("UserName",user.Email.Split("@")[0].ToString()),
//                          }),
//                Expires = DateTime.UtcNow.AddHours(settings.ExpirySettings.TokenExpiryMinutes),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT.SecretKey)), SecurityAlgorithms.HmacSha256Signature)
//            };
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
//            var token = tokenHandler.WriteToken(securityToken);
//            output.ErrorCode = IdentityOutput.ErrorCodes.Success;
//            output.Result = new LogInOutput();
//            output.Result.Token = token;
//            output.Result.success = true;
//            output.TokenExpiryMinutes = settings.ExpirySettings.TokenExpiryMinutes;
//            output.Email = user.Email;
//            output.TokenExpiryDate = DateTime.Now.AddMinutes(settings.ExpirySettings.TokenExpiryMinutes);
//            output.UserName = user.Email.Split("@")[0];
//            return output;
//        }

//        public async Task<bool> LogOut()
//        {

//            var user = await _userManager.GetUserByIdAsync(globalInfo.CreatUser);
//            if (user == null)
//                throw new AppException(SME_Core.ExceptionEnum.RecordNotExist);
//            await _signInManager.SignOutAsync();
//            var userSessions = _sessionServices.GetUsrerAllSessions(globalInfo.CreatUser);
//            if (userSessions?.Count > 0)
//            {
//                foreach (var item in userSessions)
//                {
//                    await _sessionServices.Removeasync(item);
//                    await _cacheManager.RemoveAsync(item.SessionId);
//                }
//            }
//            return true;
//        }

//        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser<string> user,string sessionId)
//        {
//            var userClaims = await _userManager.GetClaimsAsync(user);
//            var roles = await _userManager.GetRolesAsync(user);
//            var roleClaims = new List<Claim>();

//            foreach (var role in roles)
//                roleClaims.Add(new Claim("roles", role));

//            var claims = new[]
//            {
//                        new Claim("UserId",user.Id.ToString()),
//                        new Claim("SessionId",sessionId),
//                        new Claim("Email",user.Email.ToString()),
//                        new Claim("UserName",user.UserName.ToString()),
//            }
//            .Union(userClaims)
//            .Union(roleClaims);

//            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT.SecretKey));
//            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

//            var jwtSecurityToken = new JwtSecurityToken(
//                issuer: settings.JWT.Issuer,
//                audience: settings.JWT.Audience,
//                claims: claims,
//                expires: DateTime.Now.AddDays(settings.ExpirySettings.TokenExpiryMinutes),
//                signingCredentials: signingCredentials);

//            return jwtSecurityToken;
//        }

//        public async Task<bool> ResetPassword(string email)
//        {
//            var user = await _userManager.FindByEmailAsync(email);
//            if (user == null) 
//                throw new AppException(SME_Core.ExceptionEnum.UserNotFound);

//            var newSession = new SessionStatus
//            {
//                SessionId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15),
//                UserId = user.Id,
//                UserIP = _utilities.GetUserIP(),
//                UserAgent = _utilities.GetUserAgent(),
//                MacAddress = _utilities.GetMacAddress(),
//                DeviceName = _utilities.GetUserIP()
//            };
//            await _sessionServices.AddSessionAsync(newSession);

//            var token =await GetToken(user.Id, newSession.SessionId);
//            if(token == null || token.ErrorCode!=IdentityOutput.ErrorCodes.Success)
//                throw new AppException(SME_Core.ExceptionEnum.GenricError);              

//            var Url = settings.MailSettings.IdentityUrlResetPassword + "&Id=" + token.Result?.Token;

//            MailBodyModel mailbody = new MailBodyModel()
//            {
//             //   Lang= globalInfo.lang,
//                SMSMethod="",
//            };
//            string subject = "";//_mailService.CreateMailSubject(mailbody);
//            string body = "";//await _mailService.CreateMailBody(mailbody);
//            body= body?.Replace("#link#", Url);
//            var res = true;//await _mailService.SendMailAsync("mahmoudsharaf97@gmail.com", body, subject);
//            if (!res)
//                throw new AppException(SME_Core.ExceptionEnum.SendMailFailed);
//            return true;
//        }
//        public async Task<bool> ConfirmResetPassword(ResetPasswordInput input)
//        {
//            var user = await _userManager.FindByIdAsync("");

//            if (user == null)
//                throw new AppException(SME_Core.ExceptionEnum.UserNotFound);

//            var result = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
//            if (!result.Succeeded)
//            {
//                if (result.Errors.ToList()[0].Code == "PasswordTooShort")
//                    throw new AppException(SME_Core.ExceptionEnum.PasswordFormatNotValid);

//                if (result.Errors.ToList()[0].Code == "PasswordRequiresUpper")
//                    throw new AppException(SME_Core.ExceptionEnum.PasswordRequiresUpper);

//                if (result.Errors.ToList()[0].Code == "PasswordRequiresNonAlphanumeric")
//                    throw new AppException(SME_Core.ExceptionEnum.PasswordRequiresNonAlphanumeric);

//                if (result.Errors.ToList()[0].Code == "PasswordRequiresDigit")
//                    throw new AppException(SME_Core.ExceptionEnum.PasswordRequiresDigit);

//                throw new AppException(SME_Core.ExceptionEnum.RecordUpdateFailed);
//            }
           
//            return true;
//        }
//    }
//}







