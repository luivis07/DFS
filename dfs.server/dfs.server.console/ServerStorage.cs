using dfs.core.common.models;

namespace dfs.server.console;
public static class ServerStorage
{
    private static ICollection<Document> Documents { get; set; } = new List<Document>();
    public static void SetFileInfo(GetAllFileInfoMessage? getAllFileInfoMessage)
    {
        if (getAllFileInfoMessage != null)
            Documents = getAllFileInfoMessage.Documents.Select(d => d).ToList();
    }
}