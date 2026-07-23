using Cybersoft_store.Infrastructure.Models;

public interface ICategoryRepository : IRepository<Category>
{
}

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
