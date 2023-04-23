using System.Text;
using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.helpers;
using dfs.core.common.models;
using NetCoreServer;

namespace dfs.core.common.network;

public abstract class BaseSession : TcpSession
{
    private readonly TcpServer _server;
    protected IMessageProcessor? _currentMessageProcessor;
    public BaseSession(TcpServer server) : base(server)
    {
        _server = server;
    }
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        if (message.IsJson())
        {
            var baseMessage = JsonSerializer.Deserialize<BaseMessage>(message);
            if (baseMessage == null)
            {
                _server.Multicast("Message Failed");
                return;
            }
            if (baseMessage.MessageType == MessageType.SIMPLE)
            {
                var session = _server.FindSession(baseMessage.SessionId);
                var simpleMessage = JsonSerializer.Deserialize<SimpleMessage>(baseMessage.Payload);
                if (simpleMessage != null)
                    session.SendAsync($"(admin) '{simpleMessage.TextMessage}' - received.");
            }
            OnReceived(baseMessage);
        }
        else
        {
            byte[] bytes = new byte[size];
            System.Buffer.BlockCopy(buffer, 0, bytes, 0, (int)size);
            OnReceived(bytes);
        }
        base.OnReceived(buffer, offset, size);
    }
    protected abstract void OnReceived(BaseMessage baseMessage);
    protected virtual void OnReceived(byte[] buffer)
    {

    }
}