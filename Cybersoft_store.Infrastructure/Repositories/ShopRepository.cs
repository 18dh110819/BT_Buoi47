using Cybersoft_store.Infrastructure.Models;

public interface IShopRepository : IRepository<Shop>
{
}

public class ShopRepository : BaseRepository<Shop>, IShopRepository
{
    public ShopRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
