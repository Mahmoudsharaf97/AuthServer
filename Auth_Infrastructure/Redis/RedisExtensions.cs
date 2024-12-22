using Auth_Core;
using Auth_Core.UseCase.Redis;
using Auth_Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


namespace SME_Infrastructure.Redis
{
    public static class RedisExtensions
    {
        public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration, AppSettingsConfiguration appSettings)
        {

            if (appSettings.RedisEnabled)
                services.AddDependencies(appSettings);
            services.AddSingleton<IRedisCaching, RedisCaching>();
        }
        private static void AddDependencies(this IServiceCollection services, AppSettingsConfiguration appSettings)
        {
            try
            {
                ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(appSettings.RedisConnectionString);
                services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            }
            catch
            { }

        }    
    }
}