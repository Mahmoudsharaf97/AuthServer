using Auth_Core;
using Microsoft.Extensions.DependencyInjection;
using Auth_Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Auth_Infrastructure
{
    public static class ContextInjection
    {
        public static IServiceCollection AddAuthContextInjection(this IServiceCollection services,
            AppSettingsConfiguration settings)
        {
			// For  Identity
			services.AddDbContext<AuthContext>(
                m => m.UseSqlServer(settings.AuthConnectionStringDB), ServiceLifetime.Singleton);

			services.AddIdentity<ApplicationUser<string>, ApplicationRole<string>>()
		        .AddEntityFrameworkStores<AuthContext>()
		        .AddDefaultTokenProviders();
			return services;

        }
    }
}
