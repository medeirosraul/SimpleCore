using Microsoft.Extensions.Logging;
using SimpleCore.Base.Services;
using SimpleCore.Data;
using SimpleCore.Identities.Entities;
using System.Security.Claims;

namespace SimpleCore.Identities.Services;

public interface IIdentityService<TIdentity> : IService<TIdentity>
    where TIdentity : Identity, new()
{
    Task<TIdentity> GetIdentityById(string id);

    Task<TIdentity> GetIdentityForHttpContextUser(ClaimsPrincipal user);

    Task<TIdentity> CreateIdentityForIdentityProvided(IdentityProvided identityProvided);
}

public class IdentityService<TIdentity> : Service<TIdentity>, IIdentityService<TIdentity>
    where TIdentity : Identity, new()
{
    private readonly ILogger<IdentityService<TIdentity>> _logger;
    private readonly IIdentityProvidedService _identityProvidedService;

    public IdentityService(SimpleDbContext context, ILogger<IdentityService<TIdentity>> logger, IIdentityProvidedService identityProvidedService) : base(context)
    {
        _logger = logger;
        _identityProvidedService = identityProvidedService;
    }

    

    public Task<TIdentity> GetIdentityById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<TIdentity> CreateIdentityForIdentityProvided(IdentityProvided identityProvided)
    {
        var identity = new TIdentity { IsValidUserInfo = false };
        
        await Insert(identity);

        identityProvided.UserId = identity.Id;

        await _identityProvidedService.Insert(identityProvided);

        return identity;
    }

    public async Task<TIdentity> GetIdentityForHttpContextUser(ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var issuer = user.FindFirstValue("iss");
        TIdentity? identity;
        IdentityProvided? identityProvided;

        if (string.IsNullOrEmpty(sub) || string.IsNullOrEmpty(issuer))
            throw new Exception("Invalid identity provided.");

        identityProvided = await _identityProvidedService.GetBySubAndIssuer(sub, issuer);

        if (identityProvided == null)
        {
            identityProvided = new IdentityProvided(sub, issuer);
            identity = await CreateIdentityForIdentityProvided(identityProvided);
        }
        else
        {
            identity = await GetById(identityProvided.UserId!);
            if (identity == null)
                throw new Exception("Invalid identity provided.");
        }

        return identity;
    }
}
