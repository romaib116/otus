using Microsoft.Extensions.Logging;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ILogger _logger;
        protected IList<T> Data { get; set; }

        public InMemoryRepository(IList<T> data, ILogger<T> logger)
        {
            Data = data;
            _logger = logger;
        }

        public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(Data);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public async Task<Guid> SaveAsync(T entity, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();
            entity.Id = newId;
            Data.Add(entity);
            return await Task.FromResult(newId);
        }

        private Task<bool> IsExists(Guid id)
        {
            return Task.FromResult(Data.Any(x => x.Id == id));
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (await IsExists(entity.Id))
            {
                await Task.Run(() =>
                {
                    var index = Data.IndexOf(Data.FirstOrDefault(x => x.Id == entity.Id));
                    Data[index] = entity;
                }, cancellationToken);
            }
            else
            {
                throw new NotFoundException(entity.Id.ToString());
            }
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            if (await IsExists(id))
            {
                await Task.Run(() =>
                {
                    var item = Data.FirstOrDefault(x => x.Id == id);
                    if (item != null)
                        Data.Remove(item);
                }, cancellationToken);
            }
            else
            {
                throw new NotFoundException(id.ToString());
            }
        }
    }
}