using GeekShopping.IdentityServer.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Model.Context
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext() { }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

    }
}
