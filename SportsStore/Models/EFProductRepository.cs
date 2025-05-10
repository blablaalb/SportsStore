
namespace SportsStore.Models;

public class EFProductRepository(ApplicationDbContext context) : IProductRepository
{
    public IEnumerable<Product> Products => context.Products;
}
