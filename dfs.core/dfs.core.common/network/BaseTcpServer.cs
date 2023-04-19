using System.Net;
using System.Text.Json;
using dfs.core.common.models;
using NetCoreServer;

namespace dfs.core.common.network;

public abstract class BaseTcpServer : TcpServer
{
    protected BaseTcpServer(IPAddress address, int port) : base(address, port)
    {
    }
    protected override TcpSession CreateSession()
    {
        return GetSession();
    }
    protected abstract TcpSession GetSession();
    protected override void OnConnected(TcpSession session)
    {
        var baseMessage = new BaseMessage
        {
            SessionId = session.Id
        };
        var message = JsonSerializer.Serialize(baseMessage);
        session.SendAsync(message);
        base.OnConnected(session);
    }
}