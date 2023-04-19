using System.Net;
using System.Net.Sockets;
using System.Text;
using dfs.core.common.helpers;
using dfs.core.common.models;
using dfs.core.common.settings;

namespace dfs.datastore.console;
public class Runner
{
    private readonly DatastoreSettings _datastore;
    private readonly DatastoreServer _datastoreServer;
    public Runner()
    {
        _datastore = SettingsReader.GetSettings().Datastore;
        _datastoreServer = new DatastoreServer(IPAddress.Any, _datastore.Port);
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

    public void Begin()
    {
        Console.Write("Server starting...");
        
        _datastoreServer.Start();

        Console.WriteLine("Done!");

        Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

        // Perform text input
        for (; ; )
        {
            string line = Console.ReadLine() ?? "none";
            if (string.IsNullOrEmpty(line))
                break;

            // Restart the server
            if (line == "!")
            {
                Console.Write("Server restarting...");
                _datastoreServer.Restart();
                Console.WriteLine("Done!");
                continue;
            }

            // Multicast admin message to all sessions
            line = "(admin) " + line;
            _datastoreServer.Multicast(line);
        }

        // Stop the server
        Console.Write("Server stopping...");
        _datastoreServer.Stop();
        Console.WriteLine("Done!");
    }
}