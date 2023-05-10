using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleCore.Base.Entities;
using SimpleCore.Identities.Entities;

namespace SimpleCore.Data
{
    public class SimpleDbContext : DbContext
    {
        private IDbContextTransaction? _transaction;

        public SimpleDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Ignore base entity
            builder.Ignore<Entity>();
            builder.Ignore<UserEntity>();
            builder.Ignore<TenantEntity>();

            // Base
            base.OnModelCreating(builder);
        }

        protected virtual void BuildIdentity<TIdentity>(ModelBuilder builder)
            where TIdentity : Identity
        {
            builder.Entity<TIdentity>()
                .ToTable("Identity");

            builder.Entity<IdentityProvided>()
                .ToTable("IdentityProvided");
        }

        public async Task BeginTransaction()
        {
            _transaction = await Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            if (_transaction == null)
                throw new NullReferenceException("Has no transaction to commit.");

            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task Rollback()
        {
            if (_transaction == null)
                throw new NullReferenceException("Has no transaction to rollback.");

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }
}
