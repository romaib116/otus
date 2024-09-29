using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _dbContext;
        public EfRepository(SQLiteDatabaseContext dbContext) => _dbContext = dbContext;

        public async Task<Guid> CreateAsync(T entity, CancellationToken ct = default)
        {
            entity.Id = Guid.NewGuid();
            await _dbContext.Set<T>().AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
        {
            await _dbContext.Set<T>().Where(x => x.Id == guid).ExecuteDeleteAsync(ct);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.Set<T>().ToListAsync(ct);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            var dbEntity = await GetByIdAsync(entity.Id);
            if (dbEntity is not null)
            {
                _dbContext.Entry(dbEntity).CurrentValues.SetValues(entity);
                _dbContext.Entry(dbEntity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync(ct);
            }
        }
    }
}
