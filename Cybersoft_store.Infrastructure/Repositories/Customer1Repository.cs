using Cybersoft_store.Infrastructure.Models;

public interface ICustomer1Repository : IRepository<Customer1>
{
}

public class Customer1Repository : BaseRepository<Customer1>, ICustomer1Repository
{
    public Customer1Repository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
