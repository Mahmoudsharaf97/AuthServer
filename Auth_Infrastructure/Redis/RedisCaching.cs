using Auth_Core;
using Auth_Core.UseCase.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace Auth_Infrastructure.Redis
{
    public class RedisCaching : IRedisCaching
    {
            private readonly ConfigurationOptions configuration = null;
           private readonly AppSettingsConfiguration _appSettings;
            private Lazy<IConnectionMultiplexer> _Connection = null;
        private bool disposedValue;

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

        public bool SetUser(string email,string ninKey, ApplicationUser<string> user)
        {
            if ( user != null)
            {
                var tasks = new List<Task> {
                    Database.StringSetAsync( $"User_{email}", JsonConvert.SerializeObject(user)),
                    Database.StringSetAsync($"User_{ninKey}", JsonConvert.SerializeObject(user))
                };
                Task.WaitAll(tasks);
                return true;
			}
            else return false;
        }

        public async Task<ApplicationUser<string>> GetUserAsync(string emailOrNinKey)
        {
            string key = $"User_{emailOrNinKey}";
            string rv = await Database.StringGetAsync(key);
            if (rv is null)
                return null;

            var user = JsonConvert.DeserializeObject<ApplicationUser<string>>(rv)!;
            if (user is null || string.IsNullOrEmpty(user.Email))// add || string.IsNullOrEmpty(user.Nin)
				return null;

            return user;
        }

        public Task<bool> DeleteSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }

		public async Task<bool> DeleteAsync(string key)
		{
			return await Database.KeyDeleteAsync(key);
		}
	}





}

