using Cybersoft_store.Infrastructure.Models;

public interface IProductImageRepository : IRepository<ProductImage>
{
}

public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
