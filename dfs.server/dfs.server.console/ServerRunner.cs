using System.Net;
using dfs.cache.console;
using dfs.core.common.settings;
using dfs.datastore.console;

namespace dfs.server.console;

public class ServerRunner
{
    private readonly ServerSettings _server;
    private readonly DocumentServer _documentServer;

    public ServerRunner()
    {
        _server = SettingsReader.GetSettings().Server;
        _documentServer = new DocumentServer(IPAddress.Any, _server.Port);
        ServerStorage.Init();
    }
    public void Begin()
    {
        Console.Write("Server starting...");
        _documentServer.Start();
        Console.WriteLine("Done!");

        while (_documentServer.IsStarted)
        {
            string line = Console.ReadLine() ?? "none";
            if (string.IsNullOrEmpty(line))
                break;
        }

        Console.Write("Server stopping...");
        Console.Write("Clearing cache...");
        CacheStorage.ClearCache();
        _documentServer.Stop();
        Console.WriteLine("Done!");
    }
}