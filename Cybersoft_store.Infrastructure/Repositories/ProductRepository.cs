using Cybersoft_store.Infrastructure.Models;

public interface IProductRepository : IRepository<Product>
{
}

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
