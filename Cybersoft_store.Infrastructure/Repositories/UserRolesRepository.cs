using Cybersoft_store.Infrastructure.Models;

public interface IUserRolesRepository : IRepository<UserRole>
{
    // Define methods specific to UserRolesRepository if needed
}

public class UserRolesRepository : BaseRepository<UserRole>, IUserRolesRepository
{
    public UserRolesRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}