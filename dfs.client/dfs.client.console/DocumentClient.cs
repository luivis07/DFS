using System.Net;
using System.Text.Json;
using dfs.client.console.messageprocessors;
using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.core.common.network;

namespace dfs.client.console;

public class DocumentClient : BaseTcpClient
{
    private readonly MessageProvider _messageProvider;
    public DocumentClient(IPAddress address, int port, MessageProvider messageProvider) : base(address, port)
    {
        _messageProvider = messageProvider;
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
        _currentMessageProcessor = _messageProvider.GetMessageProcessor(baseMessage, typeof(DocumentClient));
        var result = _currentMessageProcessor?.ProcessMessage(baseMessage) ?? ProcessMessageStatus.Error;
        if (result == ProcessMessageStatus.Processed)
        {
            var followUpMessage = _currentMessageProcessor?.FollowUpMessage();
            if (followUpMessage != null && !string.IsNullOrEmpty(followUpMessage.FollowUpText))
            {
                SendAsync(followUpMessage.FollowUpText);
            }
        }
        else if (result == ProcessMessageStatus.Error || result == ProcessMessageStatus.Reset)
        {
            GetDocuments();
        }
        else if (result == ProcessMessageStatus.Stop)
        {
            End();
        }
    }
    protected override void OnReceived(byte[] buffer)
    {
        var result = _currentMessageProcessor?.ProcessMessage(buffer);
        if (result == ProcessMessageStatus.Processed)
        {
            GetDocuments();
        }
    }
    public void GetDocuments()
    {
        var baseMessage = new BaseMessage
        {
            SessionId = _sessionId,
            MessageType = MessageType.GET_ALL_FILEINFO
        };
        SendAsync(baseMessage.AsJson());
    }
    public void End()
    {
        Console.Write("Client disconnecting...");
        DisconnectAndStop();
        Console.WriteLine("Done!");
    }
}