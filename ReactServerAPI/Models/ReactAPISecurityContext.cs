
namespace ReactServerAPI.Models
{
    public class ReactAPISecurityContext : IdentityDbContext<IdentityUser>
    {
        public ReactAPISecurityContext(DbContextOptions<ReactAPISecurityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
