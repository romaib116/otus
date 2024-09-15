using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Guid> SaveAsync(T entity, CancellationToken cancellationToken);

        Task UpdateAsync(T entity, CancellationToken cancellationToken);

        Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    }
}