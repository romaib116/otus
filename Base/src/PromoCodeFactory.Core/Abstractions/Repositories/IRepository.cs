﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<IList<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<Guid> SaveAsync(T entity);

        Task UpdateAsync(T entity);

        Task RemoveAsync(Guid id);
    }
}