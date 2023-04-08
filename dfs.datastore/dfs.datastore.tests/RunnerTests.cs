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

    [TestMethod]
    public void GetFiles_ReturnsNotNull()
    {
        var files = _runner.GetFiles();
        Assert.IsNotNull(files);
    }

    [TestMethod]
    public void GetFileInfo_ReturnsMetadataInfo()
    {
        var fileNamesExpected = SettingsReader.GetSettings().Server.Documents.Select(d => d.Name).ToList();
        var actual = _runner.GetFileInfo();
        CollectionAssert.AreEquivalent(fileNamesExpected, actual.Select(a => a.Name).ToList());
    }

}