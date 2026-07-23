using Cybersoft_store.Infrastructure.Models;

public interface ICartItemRepository : IRepository<CartItem>
{
}

public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
{
    public CartItemRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
