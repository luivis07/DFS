using System.Net;
using System.Text.Json;
using dfs.core.common.helpers;
using dfs.core.common.models;
using dfs.core.common.network;
using NetCoreServer;
namespace dfs.datastore.console;

public class DatastoreServer : BaseTcpServer
{
    public DatastoreServer(IPAddress address, int port) : base(address, port)
    {
    }
    private static string BASE_PATH() => PathProvider.GetDatastoreBasePath();
    public IEnumerable<string> GetFiles()
    {
        var fileEntries = Directory.GetFiles(BASE_PATH());
        return fileEntries;
    }

    public IEnumerable<Document> GetFileInfo()
    {
        var filePaths = GetFiles();
        var result = new List<Document>();
        foreach (var path in filePaths)
        {
            var temp = new FileInfo(path);
            result.Add(new Document
            {
                NameWithExtension = temp.Name,
                Size = temp.Length,
                FullPath = temp.FullName
            });
        }
        return result;
    }

    public bool FileExists(string fullPath)
    {
        return File.Exists(fullPath);
    }

    public async Task<IEnumerable<byte>> GetFileContents(string fullPath)
    {
        if (FileExists(fullPath))
        {
            return await File.ReadAllBytesAsync(fullPath);
        }
        return Enumerable.Empty<byte>();
    }

    protected override TcpSession GetSession()
    {
        return new DatastoreSession(this);
    }
}