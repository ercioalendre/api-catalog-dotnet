using APICatalog.Context;
using APICatalog.Models;

namespace APICatalog.Repository;

public class ProductRepository : Repository<ProductModel>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {

    }
}
