using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.client.console.messageprocessors;

public class MessageProvider
{
    private IEnumerable<IMessageProcessor> _messageProcessors { get; set; }
    public MessageProvider()
    {
        _messageProcessors = new List<IMessageProcessor>
        {
            new GetAllFileInfoClientProcessor()
        };
    }
    public IMessageProcessor? GetMessageProcessor(BaseMessage baseMessage, Type sender)
    {
        return _messageProcessors.FirstOrDefault(m => m.AppliesTo(baseMessage, sender));
    }
}