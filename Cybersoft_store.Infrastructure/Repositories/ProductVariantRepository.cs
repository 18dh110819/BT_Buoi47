using Cybersoft_store.Infrastructure.Models;

public interface IProductVariantRepository : IRepository<ProductVariant>
{
}

public class ProductVariantRepository : BaseRepository<ProductVariant>, IProductVariantRepository
{
    public ProductVariantRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
