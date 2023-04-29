using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleCore.Entities;
using SimpleCore.Identity;

namespace SimpleCore.Data
{
    public class SimpleDbContext<TKey> : DbContext
    {
        private IDbContextTransaction? _transaction;

        public SimpleDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Ignore base entity
            builder.Ignore<Entity<TKey>>();
            builder.Ignore<UserEntity<TKey>>();
            builder.Ignore<TenantEntity<TKey>>();

            // Base
            base.OnModelCreating(builder);
        }

        protected void BuildSimpleIdentity(ModelBuilder builder)
        {
            builder.Entity<SimpleIdentity<TKey>>()
                .ToTable("Identity");

            builder.Entity<SimpleIdentityProvided<TKey>>()
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
