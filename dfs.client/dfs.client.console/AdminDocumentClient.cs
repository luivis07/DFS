using System.Net;
using dfs.client.console.messageprocessors;
using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.core.common.network;

namespace dfs.client.console;

public class AdminDocumentClient : BaseTcpClient
{
    private readonly AdminMessageProvider _adminMessageProvider;

    public bool DisplayPrompt = false;

    public AdminDocumentClient(IPAddress address, int port, AdminMessageProvider adminMessageProvider) : base(address, port)
    {
        _adminMessageProvider = adminMessageProvider;
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
        _currentMessageProcessor = _adminMessageProvider.GetMessageProcessor(baseMessage, typeof(AdminDocumentClient));
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
            PresentPrompt();
        }
        else if (result == ProcessMessageStatus.Stop)
        {
            End();
        }
    }

    public void PresentPrompt()
    {
        DisplayPrompt = false;
        var followUpMessage = ClientUI.DisplayOptions();
        if (followUpMessage == null)
        {
            Console.Write("Client disconnecting...");
            DisconnectAndStop();
            Console.WriteLine("Done!");
            return;
        }
        else if (followUpMessage.Document != null && followUpMessage.GetFollowUpContent() != null)
        {
            var reply = new BaseMessage
            {
                SessionId = _sessionId,
                Payload = followUpMessage.Document.AsJson(),
                MessageType = followUpMessage.GetMessageType()
            };
            SendAsync(reply.AsJson());
            SendAsync(followUpMessage.GetFollowUpContent());
            DisplayPrompt = true;
        }
    }

    public void End()
    {
        Console.Write("Client disconnecting...");
        DisconnectAndStop();
        Console.WriteLine("Done!");
    }
}