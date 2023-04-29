using SimpleCore.Identity;

namespace SimpleCore.Abstractions.Identity
{
    public interface IIdentityService<TIdentity, TKey> : IService<TIdentity, TKey>
        where TIdentity : Identity<TKey>
    {
        Task<TIdentity> GetIdentityById(TKey id);
        Task<TIdentity> GetIdentityForIdentityProvided(IdentityProvided<TKey> identityProvided);
    }
}
