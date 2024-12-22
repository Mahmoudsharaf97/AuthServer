using Auth_Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Auth_Core.Global;
namespace Auth_Infrastructure.Context
{
        public class AuthContext : IdentityDbContext<ApplicationUser<string>, ApplicationRole<string>, string>
        {
            private readonly GlobalInfo globalInfo;
            public AuthContext(DbContextOptions<AuthContext> options  , GlobalInfo globalInfo ) : base(options)
            {
                this.globalInfo = globalInfo;
            }
            public DbSet<ApplicationUser<string>> Users { get; set; }
            public DbSet<ApplicationRole<string>> Roles { get; set; }
            public DbSet<SessionStatus> SessionStatus { get; set; }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }
            #region SaveChanges 
            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            #endregion
        }
    }

