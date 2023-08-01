using APICatalog.Context;
using APICatalog.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalog.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public void CreateOne(T entity) { 
        _context.Add(entity);
    }

    public async Task<T?> GetOneByIdAsync(Expression<Func<T, bool>> predicate) {
        return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
    }

    public async Task<PagedList<T>> GetManyAsync(QueryStringParameters queryStringParameters)
    {
        var recordList = _context.Set<T>().AsNoTracking();

        return await PagedList<T>.ToPagedListAsync(recordList, queryStringParameters.PageNumber, queryStringParameters.PageSize);
    }

    public void UpdateOne(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        _context.Set<T>().Update(entity);
    }

    public void DeleteOne(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
