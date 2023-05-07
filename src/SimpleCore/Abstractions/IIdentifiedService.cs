using SimpleCore.Entities;

namespace SimpleCore.Abstractions
{
    public interface IIdentifiedService<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : UserEntity<TKey>
    {
    }
}
