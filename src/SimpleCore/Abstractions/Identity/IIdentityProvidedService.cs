using SimpleCore.Identity;

namespace SimpleCore.Abstractions.Identity
{
    public interface IIdentityProvidedService<TKey> : IService<IdentityProvided<TKey>, TKey>
    {
        Task<IdentityProvided<TKey>?> GetBySub(string sub);
    }
}
