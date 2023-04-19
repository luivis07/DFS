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
        if (baseMessage.MessageType == MessageType.GET_ALL_FILEINFO)
        {
            var session = _server.FindSession(baseMessage.SessionId);
            var getAllFileInfoMessage = new GetAllFileInfoMessage
            {
                Documents = ServerStorage.GetDocuments()
            };
            session?.SendAsync(baseMessage.Reply(getAllFileInfoMessage.AsJson()).AsJson());
        }
    }
}