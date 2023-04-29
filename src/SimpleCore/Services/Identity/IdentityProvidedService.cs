using Microsoft.EntityFrameworkCore;
using SimpleCore.Abstractions.Identity;
using SimpleCore.Data;
using SimpleCore.Identity;

namespace SimpleCore.Services.Identity
{
    public class IdentityProvidedService<TKey> : Service<IdentityProvided<TKey>, TKey>, IIdentityProvidedService<TKey>
    {
        public IdentityProvidedService(SimpleDbContext<TKey> context) : base(context)
        {
        }

        public Task<IdentityProvided<TKey>?> GetBySub(string sub)
        {
            var query = PrepareQuery();

            query = query.Where(x => x.Sub == sub);

            return query.FirstOrDefaultAsync();
        }
    }
}
