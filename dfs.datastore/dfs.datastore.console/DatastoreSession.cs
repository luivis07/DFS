using System.Text.Json;
using dfs.core.common.models;
using dfs.core.common.network;

namespace dfs.datastore.console;

public class DatastoreSession : BaseSession
{
    private readonly DatastoreServer _server;

    public DatastoreSession(DatastoreServer server) : base(server)
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
                Documents = _server.GetFileInfo().ToList()
            };
            session?.SendAsync(baseMessage.Reply(getAllFileInfoMessage.AsJson()).AsJson());
        }
    }
}