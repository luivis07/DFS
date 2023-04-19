using dfs.core.common.models;
using dfs.core.common.network;
using NetCoreServer;

namespace dfs.server.console;

public class DocumentServerSession : BaseSession
{
    private readonly DocumentServer _server;

    public DocumentServerSession(DocumentServer server) : base(server)
    {
        _server = server;
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
    }
}