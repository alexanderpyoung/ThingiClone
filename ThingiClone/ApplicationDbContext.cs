using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThingiClone.Models.Things;

namespace ThingiClone
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Thing> Thing { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<License> License { get; set; }
        public DbSet<Attachment> Attachment { get; set; }
    }
}
