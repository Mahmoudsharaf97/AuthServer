using Auth_Application.Interface;
using Auth_Application.Services;
using Auth_Application.Services.Caching;
using Auth_Application.Services.Captch;
using Auth_Application.Services.Token;
using IdentityApplication.Interface;
using IdentityApplication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth_Application.DependencyInjection
{
	public static class ApplicationDependencyInjectionRegistration
	{
		public static IServiceCollection AddApplicationServicesInjection(this IServiceCollection services)
		{

			services.AddScoped<IIdentityCachingServices, IdentityCachingServices>();
			services.AddScoped<IIdentityServices, IdentityServices>();
			services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
			services.AddScoped<ISessionServices, SessionServices>();
			services.AddScoped<ITokenServices, TokenServices>();
			services.AddScoped<ICaptchService, CaptchService>();
			return services;
		}

	}
}
