using Microsoft.Extensions.Logging;
using SimpleCore.Abstractions;
using SimpleCore.Abstractions.Identity;
using SimpleCore.Identity;

namespace SimpleCore.Services.Identity
{
    public class SimpleIdentityService<TKey> : ISimpleIdentityService<TKey>
    {
        private readonly ILogger<SimpleIdentityService<TKey>> _logger;
        private readonly IService<SimpleIdentity<TKey>, TKey> _identityService;
        private readonly IService<SimpleIdentityProvided<TKey>, TKey> _identityProvidedService;

        public SimpleIdentityService(ILogger<SimpleIdentityService<TKey>> logger, IService<SimpleIdentity<TKey>, TKey> identityService, IService<SimpleIdentityProvided<TKey>, TKey> identityProvidedService)
        {
            _logger = logger;
            _identityService = identityService;
            _identityProvidedService = identityProvidedService;
        }

        public Task<SimpleIdentity<TKey>> GetIdentityById(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<SimpleIdentity<TKey>> GetIdentityByIdentityProvidedSub(string sub)
        {
            throw new NotImplementedException();
        }
    }
}
