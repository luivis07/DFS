using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.core.common.network;
using dfs.server.console.messageprocessors;
using dfs.server.console.recovery;
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

    protected override bool OnReceiving(BaseMessage baseMessage)
    {
        return _server.RecoveryManager.Save(baseMessage);
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
        var session = _server.FindSession(baseMessage.SessionId);
        var result = ProcessMessageStatus.Error;
        if (session != null)
        {
            _currentMessageProcessor = _messageProvider.GetMessageProcessor(baseMessage, typeof(DocumentServerSession));
            result = _currentMessageProcessor?.ProcessMessage(baseMessage) ?? ProcessMessageStatus.Error;

            if (result == ProcessMessageStatus.Processed)
            {
                var followUpMessage = _currentMessageProcessor?.FollowUpMessage();
                if (followUpMessage != null)
                {
                    if (!string.IsNullOrWhiteSpace(followUpMessage.FollowUpText))
                        session.SendAsync(followUpMessage.FollowUpText);
                    if (followUpMessage.FollowUpContent != null && followUpMessage.FollowUpContent.Length > 0)
                        session.SendAsync(followUpMessage.FollowUpContent);
                    Console.WriteLine($"({baseMessage.SessionId}): processed - {baseMessage.MessageType}");
                }
            }
        }
        else
        {
            Console.WriteLine($"({Guid.Empty}): error - session not found");
        }
        if (result == ProcessMessageStatus.Stop)
        {
            Console.Write("Server stopping...");
            _server.Stop();
            Console.WriteLine("Done!");
        }
    }
    protected override void OnReceived(byte[] buffer)
    {
        _currentMessageProcessor?.ProcessMessage(buffer);
    }
}