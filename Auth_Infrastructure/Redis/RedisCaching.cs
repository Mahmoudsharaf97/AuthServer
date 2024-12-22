

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

          public T Get <T>(string key)
        {
            RedisValue rv =  Database.StringGet(key) ;
               if (!rv.HasValue)
                  return default;             
             return JsonConvert.DeserializeObject<T>(rv);
        }
		public async Task<T> GetAsync<T>(string key)
		{
			RedisValue rv = await Database.StringGetAsync(key);
			if (!rv.HasValue)
				return default;
			return JsonConvert.DeserializeObject<T>(rv);
		}
		public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public long Increment(string key, uint amount)
        {
            throw new NotImplementedException();
        }

        public T JsonGet<T>(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
                RedisValue rv = Database.StringGet(key, flags);
                if (!rv.HasValue)
                    return default;
                T rgv = JsonConvert.DeserializeObject<T>(rv);
                return rgv;
         }
		public async Task<T> JsonGetAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.None)
		{
			RedisValue rv = await Database.StringGetAsync(key, flags);
			if (!rv.HasValue)
				return default;
			T rgv = JsonConvert.DeserializeObject<T>(rv);
			return rgv;
		}
		public bool JsonSet(RedisKey key, object value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
                if (value == null) return false;
                return Database.StringSet(key, JsonConvert.SerializeObject(value), expiry, when, flags);
        }
		public async Task<bool> JsonSetAsync(RedisKey key, object value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
		{
			if (value == null) return false;
			return await Database.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry, when, flags);
		}
		public bool Remove(string key)
        {
            return Database.KeyDelete(key);
        }
		public async Task<bool> RemoveAsync(string key)
		{
			return await Database.KeyDeleteAsync(key);
		}

		public void RemoveAll(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value)
        {
            if(!string.IsNullOrEmpty(key) && value!=null)
            return Database.StringSet(key, JsonConvert.SerializeObject(value),TimeSpan.FromMinutes(10));
            else return false;
        }
		public async Task<bool> SetAsync<T>(string key, T value)
		{
			if (!string.IsNullOrEmpty(key) && value != null)
				return await Database.StringSetAsync(key, JsonConvert.SerializeObject(value), TimeSpan.FromMinutes(10));
			else return false;
		}
		public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
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

		public void SetAll<T>(IDictionary<string, T> values)
        {
            throw new NotImplementedException();
        }

        private void _Connection_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        //add/set cache methods removed for the sake of brevity.
        public bool Add<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public long Decrement(string key, uint amount)
        {
            throw new NotImplementedException();
        }

        public void FlushAll()
        {
            throw new NotImplementedException();
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

    }





}

