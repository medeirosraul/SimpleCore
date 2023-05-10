using Microsoft.EntityFrameworkCore;
using SimpleCore.Identities.Entities;

namespace SimpleCore.Data
{
    public class SimpleCosmosContext : SimpleDbContext
    {
        public SimpleCosmosContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void BuildIdentity<TIdentity>(ModelBuilder builder)
        {
            builder.Entity<TIdentity>()
                .ToContainer("Identities")
                .HasDiscriminator<string>("type");

            builder.Entity<TIdentity>().HasPartitionKey("UserId");


            builder.Entity<IdentityProvided>()
                .ToContainer("Identities")
                .HasDiscriminator<string>("type");

            builder.Entity<IdentityProvided>().HasPartitionKey(x => x.UserId);
        }
    }
}