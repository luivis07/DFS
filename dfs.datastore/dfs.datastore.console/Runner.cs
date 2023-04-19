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

    public void Begin()
    {
        Console.Write("Server starting...");
        _datastoreServer.Start();
        Console.WriteLine("Done!");

        while (true)
        {
            string line = Console.ReadLine() ?? "none";
            if (string.IsNullOrEmpty(line))
                break;
        }

        Console.Write("Server stopping...");
        _datastoreServer.Stop();
        Console.WriteLine("Done!");
    }
}