using dfs.core.common.settings;
using dfs.datastore.console;
namespace dfs.datastore.tests;

[TestClass]
public class RunnerTests
{
    private readonly Runner _runner;
    public RunnerTests()
    {
        _runner = new Runner();
    }
}