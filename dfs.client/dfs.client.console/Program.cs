using dfs.client.console;
var isAdmin = args.Length == 0 ? false : string.Equals(args[0], "admin", StringComparison.OrdinalIgnoreCase);
if (isAdmin)
{
    var clientRunner = new AdminClientRunner();
    clientRunner.Begin();
}
else
{
    var clientRunner = new ClientRunner();
    clientRunner.Begin();
}