using Cybersoft_store.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<IQueryable<T>> GetAllAsync();
    Task<T> GetByIdAsync(dynamic id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(dynamic id);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IQueryable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

    Task AddListAsync(List<T> entities);
    Task DeleteListAsync(List<T> entities);
}

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly CybersoftMarketPlaceContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(CybersoftMarketPlaceContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
        return await Task.FromResult(_dbSet.AsQueryable());
    }

    public async Task<T> GetByIdAsync(dynamic id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(dynamic id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    //bổ sung thêm 2 phương thức lấy 1 và lấy nhiều
    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(_dbSet.FirstOrDefault(predicate));
    }

    public async Task<IQueryable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await Task.FromResult(_dbSet.Where(predicate));
    }

    public async Task AddListAsync(List<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public async Task DeleteListAsync(List<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }
}