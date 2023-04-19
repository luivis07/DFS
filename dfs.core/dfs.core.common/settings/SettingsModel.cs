namespace dfs.core.common.settings;
public class SettingsModel
{
    public DatastoreSettings Datastore { get; set; } = new DatastoreSettings();
    public ServerSettings Server { get; set; } = new ServerSettings();
    public CacheSettings Cache { get; set; } = new CacheSettings();
    public ClientSettings Client { get; set; } = new ClientSettings();
}

public class ClientSettings
{
    public int Port { get; set; } = 1111;
    public string IPAddress { get; set; } = "127.0.0.1";
}

public class DatastoreSettings
{
    public string Path { get; set; } = string.Empty;
    public int Port { get; set; } = 1111;
}

public class ServerSettings
{
    public IEnumerable<DocumentSettings> Documents { get; set; } = Enumerable.Empty<DocumentSettings>();
    public string IPAddress { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 1111;
}

public class DocumentSettings
{
    public string Name { get; set; } = string.Empty;
    public int Cost { get; set; }
}

public class CacheSettings
{
    public int Size { get; set; } = 1000;
    public string Path { get; set; } = string.Empty;
}