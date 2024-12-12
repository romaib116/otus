using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Pcf.Administration.Core.Domain;

namespace Pcf.Administration.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
        
        Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}