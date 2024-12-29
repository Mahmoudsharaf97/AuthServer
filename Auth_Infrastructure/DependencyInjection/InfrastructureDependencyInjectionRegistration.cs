using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using Auth_Infrastructure.Identity;
using Auth_Infrastructure.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Infrastructure.DependencyInjection
{
	public static class InfrastructureDependencyInjectionRegistration
	{
		public static IServiceCollection AddInfrastructureServicesInjection(this IServiceCollection services)
		{
			services.AddSingleton<IRedisCaching, RedisCaching>();
			services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
			services.AddScoped<IUsersCachedManager, UsersCachedManager>();
			return services;
		}

	}
}
