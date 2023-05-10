using Microsoft.AspNetCore.Http;
using SimpleCore.Identities.Entities;
using SimpleCore.Identities.Services;
using System.Security.Claims;

namespace SimpleCore.Contexts
{
    public interface IIdentityContext<TIdentity>
        where TIdentity : Identity, new()
    {
        Task<TIdentity> GetUserInfo();
        Task<TIdentity> UpdateUserInfo(string userName, string email);
    }

    public class IdentityContext<TIdentity> : IIdentityContext<TIdentity>
        where TIdentity : Identity, new()
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityService<TIdentity> _identityService;

        public IdentityContext(IHttpContextAccessor contextAccessor, IIdentityService<TIdentity> identityService)
        {
            _contextAccessor = contextAccessor;
            _identityService = identityService;
        }

        public Task<TIdentity> GetUserInfo()
        {
            if (_contextAccessor.HttpContext == null)
                throw new Exception("Http context is null.");

            // Get user from HTTP Context
            var user = _contextAccessor.HttpContext.User;

            if (user == null)
                throw new Exception("No identity provided.");

            return _identityService.GetIdentityForHttpContextUser(user);
        }

        public async Task<TIdentity> UpdateUserInfo(string userName, string email)
        {
            var identity = await GetUserInfo();
            
            identity.Email = email;
            
            if (string.IsNullOrEmpty(identity.UserName))
                identity.UserName = userName;
            
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email))
                identity.IsValidUserInfo = true;

            await _identityService.Update(identity);

            return identity;
        }
    }
}