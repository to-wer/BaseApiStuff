using BaseApiStuff.Models;

namespace BaseApiStuff.Repositories;

public interface IGenericRepository<T, in TKey> where T : class where TKey : IEquatable<TKey>
{
    Task<T> GetAsync(TKey id);
    Task<List<T>> GetAllAsync();
    Task<PagedResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters) where TResult : class;
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TKey id);
    Task<bool> ExistsAsync(TKey id);
}