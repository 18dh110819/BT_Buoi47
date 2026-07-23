using Cybersoft_store.Infrastructure.Models;

public interface IRatingRepository : IRepository<Rating>
{
}

public class RatingRepository : BaseRepository<Rating>, IRatingRepository
{
    public RatingRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
