using System.Net;
using dfs.core.common.models;
using dfs.core.common.settings;
using dfs.datastore.console;

namespace dfs.server.console;
public static class ServerStorage
{
    private static readonly DatastoreServer _datastoreServer;
    private static readonly ServerSettings _serverSettings;

    static ServerStorage()
    {
        _datastoreServer = new DatastoreServer(IPAddress.Any, 0);
        _serverSettings = SettingsReader.GetSettings().Server;
        Init();
    }
    private static IEnumerable<Document> Documents { get; set; } = Enumerable.Empty<Document>();
    public static void SetFileInfo(GetAllFileInfoMessage? getAllFileInfoMessage)
    {
        if (getAllFileInfoMessage != null)
            Documents = getAllFileInfoMessage.Documents.Select(d => d).ToList();
    }
    public static IEnumerable<Document> GetDocuments() => Documents;
    public static void Init()
    {
        var documents = _datastoreServer.GetFileInfo();
        foreach (var document in documents)
        {
            var temp = _serverSettings.Documents.FirstOrDefault(d => d.Name == document.Name);
            if (temp != null)
            {
                document.Cost = temp.Cost;
                document.QuantityAvailable = temp.Quantity;
            }
        }
        Documents = documents;
    }

    public static byte[] GetDocumentContent(string? fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
            return new byte[0];

        var content = _datastoreServer.GetFileContents(fullPath);
        return content.ToArray();
    }
}