using APICatalog.Pagination;
using System.Linq.Expressions;

namespace APICatalog.Repository;

public interface IRepository<T>
{
    void CreateOne(T entity);

    Task<PagedList<T>> GetManyAsync(QueryStringParameters queryStringParameters);

    Task<T?> GetOneByIdAsync(Expression<Func<T, bool>> predicate);

    void UpdateOne(T entity);

    void DeleteOne(T entity);
}
