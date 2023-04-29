using Microsoft.AspNetCore.Http;
using SimpleCore.Abstractions.Identity;
using SimpleCore.Identity;
using System.Security.Claims;

namespace SimpleCore.Contexts
{
    public interface IIdentityContext<TIdentity, TKey>
        where TIdentity : Identity<TKey>, new()
    {
        Task<TIdentity> GetUserInfo();
    }

    public class IdentityContext<TIdentity, TKey> : IIdentityContext<TIdentity, TKey>
        where TIdentity : Identity<TKey>, new()
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityService<TIdentity, TKey> _identityService;

        public IdentityContext(IHttpContextAccessor contextAccessor, IIdentityService<TIdentity, TKey> identityService)
        {
            _contextAccessor = contextAccessor;
            _identityService = identityService;
        }

        public Task<TIdentity> GetUserInfo()
        {
            if (_contextAccessor.HttpContext == null)
                throw new Exception("Http context is null.");

            // Get user from HTTP Context
            var httpContextIdentity = _contextAccessor.HttpContext.User;

            if (httpContextIdentity == null)
                throw new Exception("No identity provided.");

            var sub = httpContextIdentity.FindFirstValue("sub");
            var issuer = httpContextIdentity.FindFirstValue("iss");

            if (string.IsNullOrEmpty(sub) || string.IsNullOrEmpty(issuer))
                throw new Exception("Invalid identity provided.");

            var identityProvided = new IdentityProvided<TKey>(sub, issuer);

            return _identityService.GetIdentityForIdentityProvided(identityProvided);
        }
    }
}