using Cybersoft_store.Infrastructure.Models;

public interface IRoleRepository : IRepository<Role>
{
}

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
