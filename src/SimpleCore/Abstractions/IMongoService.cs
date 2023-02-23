﻿using Microsoft.Extensions.Logging;
using SimpleCore.Entities;

namespace SimpleCore.Abstractions
{
    public interface IMongoService<TEntity, TKey> 
        where TEntity : Entity<TKey>
    {
        Task Insert(TEntity entity);

        Task<TEntity?> GetById(TKey id, bool tracking = false);
    }
}