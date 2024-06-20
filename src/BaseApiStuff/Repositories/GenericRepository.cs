using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApiStuff.Extensions;
using BaseApiStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseApiStuff.Repositories;

public class GenericRepository<T, TKey> : IGenericRepository<T, TKey>
    where T : class
    where TKey : IEquatable<TKey>
{
    protected readonly DbContext Context;
    protected readonly IMapper Mapper;

    protected GenericRepository(DbContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public async Task<T> GetAsync(TKey id)
    {
        if (id == null) return null;
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task<PagedResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        where TResult : class
    {
        var totalCount = await Context.Set<T>().CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)totalCount / queryParameters.PageSize);
		
		var items = await Context.Set<T>().AsQueryable()
            .Order(queryParameters.SortField, queryParameters.SortDirection)
			.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ProjectTo<TResult>(Mapper.ConfigurationProvider)
            .ToListAsync();
		
        return new PagedResponse<TResult>
        {
            Items = items, TotalCount = totalCount,
            TotalPages = totalPages,
            Page = queryParameters.Page,
            PageSize = queryParameters.PageSize
        };
    }

    public async Task<T> AddAsync(T entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Update(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TKey id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return;
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }
}