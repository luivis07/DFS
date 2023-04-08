using dfs.datastore.console;

public class Program
{
    public static async Task Main(string[] args)
    {
        var runner = new Runner();
        await runner.Begin();        
    }
}

