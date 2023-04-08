using dfs.core.common.settings;
using dfs.datastore.console;
namespace dfs.datastore.tests;

[TestClass]
public class RunnerTests
{
    private readonly Runner _runner;
    private const string REAL_FILE = @"C:\Projects\DFS\dfs.documents\datastore\A.txt";
    private const int REAL_FILE_SIZE = 76800;
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
    public async Task GetFileInfo_ReturnsMetadataInfo()
    {
        var fileNamesExpected = (await SettingsReader.GetSettings()).Server.Documents.Select(d => d.Name).ToList();
        var actual = await _runner.GetFileInfo();
        CollectionAssert.AreEquivalent(fileNamesExpected, actual.Select(a => a.Name).ToList());
    }

    [TestMethod]
    public void FileExists_ShouldReturnTrueWhenExists()
    {
        var actual = _runner.FileExists(REAL_FILE);
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void FileExists_ShouldReturnFalseWhenNotExists()
    {
        var actual = _runner.FileExists(@"C:\Projects\DFS\dfs.documents\datastore\Z.txt");
        Assert.IsFalse(actual);
    }

    [TestMethod]
    public async Task GetFileContents_ShouldReturnContentsAsBytes()
    {
        var actual = (await _runner.GetFileContents(REAL_FILE)).ToArray();
        Assert.IsNotNull(actual);
        Assert.AreEqual(REAL_FILE_SIZE, actual.Length);
    }
}