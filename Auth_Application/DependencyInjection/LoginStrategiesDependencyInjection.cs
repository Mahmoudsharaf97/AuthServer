using Microsoft.Extensions.DependencyInjection;
using Auth_Application.Interface.Login;
using Auth_Application.Services.Login.LoginNationalIdConfirmation;
using Auth_Application.Services.Login.NormalLogin;
using Auth_Application.Services.VerifyLoginOTP;
using Auth_Application.Services.Login;

namespace Auth_Application.DependencyInjection
{
	public static class LoginStrategiesDependencyInjection
	{
		public static IServiceCollection AddLoginStrategiesServicesInjection(this IServiceCollection services)
		{
			services.AddScoped<ILoginStrategy, LoginNationalIdConfirmationStrategy>();
			services.AddScoped<ILoginStrategy, EmailLoginStrategy>();
			services.AddScoped<ILoginStrategy, NationalIdLoginStrategy>();
			services.AddScoped<ILoginStrategy, EmailVerifyLoginOTPStrategy>();
			services.AddScoped<ILoginStrategy, NationalVerifyLoginOTPStrategy>();
			services.AddScoped<ILoginStrategy, VerifyLoginYakeenMobileStrategy>();
			services.AddScoped<ILoginStrategyManager, LoginStrategyManager>();

			return services;
		}
	}
}
