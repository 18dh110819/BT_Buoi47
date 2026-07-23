using Cybersoft_store.Infrastructure.Models;

public interface ICartRepository : IRepository<Cart>
{
}

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
