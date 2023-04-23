using System.Net;
using dfs.client.console.messageprocessors;
using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.core.common.network;

namespace dfs.client.console;

public class AdminDocumentClient : BaseTcpClient
{
    private readonly AdminMessageProvider _adminMessageProvider;

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
            PresentPrompt(baseMessage);
        }
        else if (result == ProcessMessageStatus.Error || result == ProcessMessageStatus.Reset)
        {
            PresentPrompt(baseMessage);
        }
        else if (result == ProcessMessageStatus.Stop)
        {
            End();
        }
    }

    private void PresentPrompt(BaseMessage baseMessage)
    {
        var followUpMessage = ClientUI.DisplayOptions();
        while (followUpMessage == null)
        {
            Console.WriteLine("Choose a valid option");
            followUpMessage = ClientUI.DisplayOptions();
        }
        if (followUpMessage.Document != null && followUpMessage.GetFollowUpContent() != null)
        {
            var reply = baseMessage.Reply(followUpMessage.Document.AsJson(), followUpMessage.GetMessageType()).AsJson();
            SendAsync(reply);
            SendAsync(followUpMessage.GetFollowUpContent());
        }
    }

    public void End()
    {
        Console.Write("Client disconnecting...");
        DisconnectAndStop();
        Console.WriteLine("Done!");
    }
}