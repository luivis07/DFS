using System.Net;
using dfs.core.common.network;
using NetCoreServer;

namespace dfs.server.console;

public class DocumentServer : TcpServer
{
    public DocumentServer(IPAddress address, int port) : base(address, port)
    {
    }
    protected override TcpSession CreateSession()
    {
        return new DocumentServerSession(this);
    }
}