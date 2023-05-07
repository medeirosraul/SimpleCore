using Microsoft.Extensions.Logging;
using SimpleCore.Abstractions.Identity;
using SimpleCore.Data;
using SimpleCore.Identity;

namespace SimpleCore.Services.Identity
{
    public class IdentityService<TIdentity, TKey> : Service<TIdentity, TKey>, IIdentityService<TIdentity, TKey>
        where TIdentity : Identity<TKey>, new()
    {
        private readonly ILogger<IdentityService<TIdentity, TKey>> _logger;
        private readonly IIdentityProvidedService<TKey> _identityProvidedService;

        public IdentityService(SimpleDbContext<TKey> context, ILogger<IdentityService<TIdentity, TKey>> logger, IIdentityProvidedService<TKey> identityProvidedService) : base(context)
        {
            _logger = logger;
            _identityProvidedService = identityProvidedService;
        }

        public Task<TIdentity> GetIdentityById(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<TIdentity> GetIdentityForIdentityProvided(IdentityProvided<TKey> identityProvided)
        {
            var identityProvidedEntity = await _identityProvidedService.GetBySub(identityProvided.Sub);

            // If identity provided doesn't exist, create one.
            if (identityProvidedEntity == null)
            {
                await _identityProvidedService.Insert(identityProvided);
                identityProvidedEntity = identityProvided;
            }

            // If user doesn't exist for identity provided, create one.
            TIdentity? identity;

            if (identityProvidedEntity!.UserId == null)
            {
                identity = new TIdentity
                {
                    IsValidUserInfo = false
                };

                await Insert(identity);
                identityProvidedEntity.UserId = identity.Id!.ToString();

                await _identityProvidedService.Update(identityProvidedEntity);
            }
            else
            {
                identity = await GetById(identityProvidedEntity.UserId);

                if (identity == null)
                {
                    var exceptionMessage = $"The identity {identityProvidedEntity.UserId} with sub {identityProvidedEntity.Sub} doesn't exist.";

                    _logger.LogError(exceptionMessage);

                    throw new Exception(exceptionMessage);
                }
            }

            return identity;
        }
    }
}
