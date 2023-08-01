using APICatalog.Context;
using APICatalog.Models;

namespace APICatalog.Repository;

public class CategoryRepository : Repository<CategoryModel>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
