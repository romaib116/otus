using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Получить все
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Получить по иду
        /// </summary>
        /// <param name="Guid">Гуид</param>
        /// <param name="ct">Токен отмены</param>
        Task<T> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Создать
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="ct">Токен отмены</param>
        Task<Guid> CreateAsync(T entity, CancellationToken ct = default);

        /// <summary>
        /// Обновить
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="ct">Токен отмены</param>
        Task UpdateAsync(T entity, CancellationToken ct = default);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="guid">Гуид</param>
        /// <param name="ct">Токен отмены</param>
        Task DeleteAsync(Guid guid, CancellationToken ct = default);
    }
}