using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.Administration.DataAccess.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase mongoDatabase) 
        {
            _mongoDatabase = mongoDatabase;
            var collectionName = typeof(T).Name;
            _collection = _mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.Id = entity.Id.Equals(Guid.Empty) ? Guid.NewGuid() : entity.Id;
            await _collection.InsertOneAsync(entity, null, cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(x => x.Id == entity.Id, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.FindAsync(x => true, null, cancellationToken).Result.ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _collection.FindAsync(x => x.Id == id, null, cancellationToken).Result.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> GetFirstWhere(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.FindAsync(predicate, null, cancellationToken).Result.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _collection.FindAsync(x => ids.Contains(x.Id), null, cancellationToken).Result.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetWhere(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.FindAsync(predicate, null, cancellationToken).Result.ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false }, cancellationToken);
        }
    }
}
