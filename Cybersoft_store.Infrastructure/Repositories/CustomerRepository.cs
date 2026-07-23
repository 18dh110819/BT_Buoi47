using Cybersoft_store.Infrastructure.Models;

public interface ICustomerRepository : IRepository<Customer>
{
}

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
