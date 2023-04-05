namespace dfs.core.common.settings;
public class SettingsModel
{
    public Datastore Datastore { get; set; } = new Datastore();
    public Server Server { get; set; } = new Server();
    public Cache Cache { get; set; } = new Cache();
}

public class Datastore
{
    public string Path { get; set; } = string.Empty;
}

public class Server
{
    public IEnumerable<Documents> Documents { get; set; } = Enumerable.Empty<Documents>();
}

public class Documents
{
    public string Name { get; set; } = string.Empty;
    public int Cost { get; set; }
}

public class Cache
{
    public int Size { get; set; } = 1000;
    public string Path { get; set; } = string.Empty;
}