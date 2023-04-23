using System.Net;
using dfs.core.common.helpers;
using dfs.core.common.models;
using dfs.core.common.settings;

namespace dfs.datastore.console;
public static class ServerStorage
{
    private static readonly DatastoreServer _datastoreServer;
    private static readonly ServerSettings _serverSettings;
    private static readonly DatastoreSettings _datastoreSettings;

    static ServerStorage()
    {
        _datastoreServer = new DatastoreServer(IPAddress.Any, 0);
        _serverSettings = SettingsReader.GetSettings().Server;
        _datastoreSettings = SettingsReader.GetSettings().Datastore;
        _documents = new List<Document>();
        Init();
    }
    private static ICollection<Document> _documents { get; set; }
    public static void SetFileInfo(GetAllFileInfoMessage? getAllFileInfoMessage)
    {
        if (getAllFileInfoMessage != null)
            _documents = getAllFileInfoMessage.Documents.Select(d => d).ToList();
    }
    public static IEnumerable<Document> GetDocuments() => _documents.Select(d => d).ToList();
    public static void Init()
    {
        var datastoreDirectory = new DirectoryInfo(_datastoreSettings.Path);
        foreach (var f in datastoreDirectory.GetFiles())
        {
            f.Delete();
        }
        var backupDirectory = PathProvider.GetDatastoreBackupPath();
        var filePaths = Directory.GetFiles(backupDirectory);
        foreach (var fileName in filePaths)
        {
            var fi = new FileInfo(fileName);
            File.Copy(fileName, Path.Combine(PathProvider.GetDatastoreBasePath(), fi.Name));
        }

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
        _documents = documents.ToList();
    }

    public static IEnumerable<byte> GetDocumentContent(string? fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
            return Enumerable.Empty<byte>();

        var content = _datastoreServer.GetFileContents(fullPath);
        return content.ToList();
    }

    public static void DecreaseQuantity(string name)
    {
        var doc = _documents.FirstOrDefault(d => string.Equals(name, d.Name, StringComparison.OrdinalIgnoreCase));
        if (doc != null)
            doc.QuantityAvailable--;
    }
    public static bool IsAvailable(string name)
    {
        var doc = _documents.FirstOrDefault(d => string.Equals(name, d.Name, StringComparison.OrdinalIgnoreCase));
        if (doc == null)
            return false;
        return doc.QuantityAvailable > 0;
    }

    public static bool AddDocument(Document document, byte[] contents)
    {
        if (!_documents.Any(d => string.Equals(d.Name, document.Name, StringComparison.OrdinalIgnoreCase)))
        {
            _documents.Add(document);
            File.WriteAllBytes(document.FullPath, contents);
            return true;
        }
        return false;
    }
    public static bool RemoveDocument(string name)
    {
        var document = _documents.FirstOrDefault(d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));
        if (document == null)
            return false;
        _documents.Remove(document);
        File.Delete(document.FullPath);
        return true;
    }
}