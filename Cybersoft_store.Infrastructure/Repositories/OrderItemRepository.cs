using Cybersoft_store.Infrastructure.Models;

public interface IOrderItemRepository : IRepository<OrderItem>
{
}

public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
