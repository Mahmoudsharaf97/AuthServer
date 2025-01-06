using Auth_Core;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SME_Core;
using StackExchange.Redis;
namespace Auth_Infrastructure.Redis
{
    public class RedisCaching : IRedisCaching
    {
       string registerBaseKey="RegisterUser" ;
       private readonly ConfigurationOptions configuration = null;
       private readonly AppSettingsConfiguration _appSettings;
       private Lazy<IConnectionMultiplexer> _Connection = null;
        private bool disposedValue;
		JsonSerializerSettings SerializSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
		public RedisCaching(AppSettingsConfiguration appSettings)
            {
            this._appSettings = appSettings;
                configuration = new ConfigurationOptions()
                {
                    //for the redis pool so you can extent later if needed
                    EndPoints = { { _appSettings.RedisConnectionString, _appSettings.RedisInstance }, }, 
                   // AllowAdmin = allowAdmin,
                    //Password = "", //to the security for the production
                    ClientName = "My Redis Client",
                    ReconnectRetryPolicy = new LinearRetry(500000000),
                    AbortOnConnectFail = false,
                };
                _Connection = new Lazy<IConnectionMultiplexer>(() =>
                {
                    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration);
                    return redis;
                });
            }
            //for the 'GetSubscriber()' and another Databases
            public IConnectionMultiplexer Connection { get { return _Connection.Value; } }

            //for the default database
            public IDatabase Database => Connection.GetDatabase();

		public async Task<T> GetAsync<T>(string key)
		{
			RedisValue rv = await Database.StringGetAsync(key);
			if (!rv.HasValue)
				return default;
			return JsonConvert.DeserializeObject<T>(rv);

		}
		public T Get <T>(string key)
        {
            RedisValue rv =  Database.StringGet(key) ;
               if (!rv.HasValue)
                  return default;             
             return JsonConvert.DeserializeObject<T>(rv);
        }
		public async Task<string> GetRefreshTokenAsync(string email)
		{
            string key = $"RefreshToken_{email}";

            string rv = await Database.StringGetAsync(key);
			if (rv is null)
				return null;
			return rv;
		}
		public async Task<bool> SetRefreshTokenAsync(string email, string tokenValue)
		{
            string key = $"RefreshToken_{email}";
            if (tokenValue != null)
				return await Database.StringSetAsync(key, tokenValue, TimeSpan.FromMinutes(_appSettings.JwtRefreshTokenExpiryMinutes));
			else return false;
		}

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            if (!string.IsNullOrEmpty(key) && value != null)
                return Database.StringSet(key, JsonConvert.SerializeObject(value),expiresIn);
            else return false;
        }
		public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn)
		{
			if (!string.IsNullOrEmpty(key) && value != null)
				return await Database.StringSetAsync(key, JsonConvert.SerializeObject(value), expiresIn);
			else return false;
		}
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public async  Task<bool> SetSessionAsync(string email, SessionStatus session)
        {
            // need to handle session by email and nationalId
            string key = $"Session_{email}";
            if (!string.IsNullOrEmpty(key) && session != null)
                return await Database.StringSetAsync(key, JsonConvert.SerializeObject(session), TimeSpan.FromMinutes(_appSettings.JwtSessionExpireInMinutes));
            else return false;
        }

        public async Task<SessionStatus> GetSessionAsync(string email)
        {
            string key = $"Session_{email}";
            string rv = await Database.StringGetAsync(key);
            if (rv is null)
                return null;

                SessionStatus session= JsonConvert.DeserializeObject<SessionStatus>(rv)!;
            if (session is null || string.IsNullOrEmpty(session.SessionId))
                return null;

            return session;
        }

        public async Task<bool> DeleteSessionAsync(string email)
        {
            string key = $"Session_{email}";
            return await Database.KeyDeleteAsync(email);
        }

        public async Task<bool> SetUser(string email,string ninKey, ApplicationUser<string> user)
        {
            if ( user != null)
            {
                var tasks = new List<Task> {
                    Database.StringSetAsync( $"UserEmail:{email.ToLower()}", JsonConvert.SerializeObject(user)),
                    Database.StringSetAsync($"UserNationalId:{ninKey}", user.Email!.ToLower())
                };
                Task.WhenAll(tasks);
                return true;
			}
            else return false;
        }

        public async Task<ApplicationUser<string>> GetUserAsync(string Key,bool IsEmail=false,bool isNin=false)
        {
            string _emailKey = $"UserEmail:{Key.ToLower()}";
            string _ninKey = $"UserNationalId:{Key}";
            string rv = string.Empty;
            if (IsEmail)
                rv = await Database.StringGetAsync(_emailKey);
            else
            {
                string emailValue= await Database.StringGetAsync(_ninKey);
                if (!string.IsNullOrEmpty(emailValue))
                {
                    rv = await Database.StringGetAsync($"UserEmail:{emailValue.ToLower()}");
                }
            }
            if (rv is null)
                return null;

            var user = JsonConvert.DeserializeObject<ApplicationUser<string>>(rv)!;
            if (user is null || string.IsNullOrEmpty(user.Email))// add || string.IsNullOrEmpty(user.Nin)
				return null;
            return user;
        }


		public async Task<bool> SetAsync<T>(string key, T value)
		{
			if (!string.IsNullOrEmpty(key) && value != null)
				return await Database.StringSetAsync(key, JsonConvert.SerializeObject(value, SerializSettings));
			else return false;
		}



        #region registraion 
        public async Task<bool> SetRegisterUserAfterPhoneVerify(ApplicationUser<string> user)
        {
           return  await SetAsync<ApplicationUser<string>>($"{registerBaseKey}_{user.Email}_{user.NationalId}_{user.PhoneNumber}", user, TimeSpan.FromMinutes(60));
        }
        public async Task<ApplicationUser<string>> GetRegisterUserAfterPhoneVerify(string userEmail, string userNin,string phone)
        {
         ApplicationUser<string> user=   await GetAsync<ApplicationUser<string>>($"{registerBaseKey}_{userEmail}_{userNin}_{phone}");
            if (user is null || string.IsNullOrEmpty(user.Email))// add || string.IsNullOrEmpty(user.Nin)
                return null;
            return user;
        }
        public async Task<bool> SetRegisterUserAfterGenerateOTP(ApplicationUser<string> user)
        {
            return await SetAsync<ApplicationUser<string>>($"{registerBaseKey}_{user.Email}_{user.NationalId}_{user.PhoneNumber}_otp", user, TimeSpan.FromMinutes(160));
        }
        public async Task<ApplicationUser<string>> GetRegisterUserAfterGenerateOTP(string userEmail, string userNin,string phone)
        {
            ApplicationUser<string> user = await GetAsync<ApplicationUser<string>>($"{registerBaseKey}_{userEmail}_{userNin}_{phone}_otp");
            if (user is null || string.IsNullOrEmpty(user.Email))
                return null;
            return user;
        }
        public async  Task<bool> DeleteAsync(string key)
        {
            return await Database.KeyDeleteAsync(key);
        }

        public async Task<bool> DeletUserRegisterTries(string userEmail, string userNin, string phone)
        {


              var tasks = new List<Task> {
                    Database.KeyDeleteAsync( $"{registerBaseKey}_{userEmail}_{userNin}_{phone}"),
                    Database.KeyDeleteAsync($"{registerBaseKey}_{userEmail}_{userNin}_{phone}_otp")
                };
             Task.WhenAll(tasks); 

                return true;
   
        }

  
        #endregion
    }





}

