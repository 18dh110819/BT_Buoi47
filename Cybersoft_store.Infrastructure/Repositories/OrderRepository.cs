using Cybersoft_store.Infrastructure.Models;

public interface IOrderRepository : IRepository<Order>
{
}

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
