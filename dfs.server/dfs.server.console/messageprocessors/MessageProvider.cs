using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.server.console.messageprocessors;

public class MessageProvider
{
    private IEnumerable<IMessageProcessor> _messageProcessors { get; set; }
    public MessageProvider()
    {
        _messageProcessors = new List<IMessageProcessor>
        {
            new GetAllFileInfoServerProcessor()
        };
    }
    public IMessageProcessor? GetMessageProcessor(BaseMessage baseMessage, Type sender)
    {
        return _messageProcessors.FirstOrDefault(m => m.AppliesTo(baseMessage, sender));
    }
}