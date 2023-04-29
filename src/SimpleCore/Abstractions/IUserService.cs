using SimpleCore.Entities;

namespace SimpleCore.Abstractions
{
    public interface IUserService<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : UserEntity<TKey>
    {

    }
}