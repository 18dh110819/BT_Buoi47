using Cybersoft_store.Infrastructure.Models;

public interface IMessageRepository : IRepository<Message>
{
}

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
