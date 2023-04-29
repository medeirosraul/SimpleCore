using SimpleCore.Identity;

namespace SimpleCore.Abstractions.Identity
{
    public interface ISimpleIdentityService<TKey>
    {
        Task<SimpleIdentity<TKey>> GetIdentityById(TKey id);
        Task<SimpleIdentity<TKey>> GetIdentityByIdentityProvidedSub(string sub);
    }
}
