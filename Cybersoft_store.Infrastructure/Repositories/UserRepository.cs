using Cybersoft_store.Infrastructure.Models;

public interface IUserRepository : IRepository<User>
{
    // Define methods specific to UserRepository if needed
    Task<int> CalculateTotalPriceAsync(Guid userId);
}

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }

    public Task<int> CalculateTotalPriceAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}