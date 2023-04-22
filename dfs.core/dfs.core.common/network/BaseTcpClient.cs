using System.Net;
using System.Text;
using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.helpers;
using dfs.core.common.models;
using NetCoreServer;

namespace dfs.core.common.network;

public abstract class BaseTcpClient : TcpClient
{
    protected bool _stop;
    public Guid _sessionId;
    protected IMessageProcessor? _currentMessageProcessor;
    protected BaseTcpClient(IPAddress address, int port) : base(address, port)
    {
    }
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        if (message.IsJson())
        {
            _currentMessageProcessor = null;
            var baseMessage = JsonSerializer.Deserialize<BaseMessage>(message);
            if (baseMessage != null)
            {
                _sessionId = baseMessage.SessionId;
                OnReceived(baseMessage);
            }
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


    public void DisconnectAndStop()
    {
        _stop = true;
        DisconnectAsync();
        while (IsConnected)
            Thread.Yield();
    }
}