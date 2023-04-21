using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.core.common.network;
using dfs.server.console.messageprocessors;
using NetCoreServer;

namespace dfs.server.console;

public class DocumentServerSession : BaseSession
{
    private readonly DocumentServer _server;
    private readonly MessageProvider _messageProvider;

    public DocumentServerSession(DocumentServer server, MessageProvider messageProvider) : base(server)
    {
        _server = server;
        _messageProvider = messageProvider;
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
        var messageProcessor = _messageProvider.GetMessageProcessor(baseMessage, typeof(DocumentServerSession));
        var result = messageProcessor?.ProcessMessage(baseMessage) ?? ProcessMessageStatus.Error;
        var session = _server.FindSession(baseMessage.SessionId);
        if (result == ProcessMessageStatus.Processed)
        {
            var followUpMessage = messageProcessor?.FollowUpMessage();
            if (!string.IsNullOrWhiteSpace(followUpMessage))
                SendAsync(followUpMessage);
        }
        if (result == ProcessMessageStatus.Stop)
        {
            Console.Write("Server stopping...");
            _server.Stop();
            Console.WriteLine("Done!");
        }
    }
}