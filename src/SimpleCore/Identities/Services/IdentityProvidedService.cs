using Microsoft.EntityFrameworkCore;
using SimpleCore.Base.Services;
using SimpleCore.Data;
using SimpleCore.Identities.Entities;

namespace SimpleCore.Identities.Services;

public interface IIdentityProvidedService : IService<IdentityProvided>
{
    Task<IdentityProvided?> GetBySubAndIssuer(string sub, string issuer);
}

public class IdentityProvidedService : Service<IdentityProvided>, IIdentityProvidedService
{
    public IdentityProvidedService(SimpleDbContext context) : base(context)
    {
    }

    public Task<IdentityProvided?> GetBySubAndIssuer(string sub, string issuer)
    {
        var query = PrepareQuery();

        query = query.Where(x => x.Sub == sub && x.Issuer == issuer);

        return query.FirstOrDefaultAsync();
    }
}
