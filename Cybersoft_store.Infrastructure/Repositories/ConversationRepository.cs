using Cybersoft_store.Infrastructure.Models;

public interface IConversationRepository : IRepository<Conversation>
{
}

public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
{
    public ConversationRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
