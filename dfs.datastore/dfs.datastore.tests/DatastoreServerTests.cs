using System.Net;
using dfs.core.common.settings;
using dfs.datastore.console;

namespace dfs.datastore.tests;

[TestClass]
public class DatastoreServerTests
{
    private const string REAL_FILE = @"C:\Projects\DFS\dfs.documents\datastore\A.txt";
    private const int REAL_FILE_SIZE = 76800;
    private readonly DatastoreServer _datastoreServer;

    public DatastoreServerTests()
    {
        _datastoreServer = new DatastoreServer(IPAddress.Any, 0);
    }

    [TestMethod]
    public void GetFiles_ReturnsNotNull()
    {
        var files = _datastoreServer.GetFiles();
        Assert.IsNotNull(files);
    }

    [TestMethod]
    public void GetFileInfo_ReturnsMetadataInfo()
    {
        var fileNamesExpected = SettingsReader.GetSettings().Server.Documents.Select(d => d.Name).ToList();
        var actual = _datastoreServer.GetFileInfo();
        CollectionAssert.AreEquivalent(fileNamesExpected, actual.Select(a => a.Name).ToList());
    }

    [TestMethod]
    public void FileExists_ShouldReturnTrueWhenExists()
    {
        var actual = _datastoreServer.FileExists(REAL_FILE);
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void FileExists_ShouldReturnFalseWhenNotExists()
    {
        var actual = _datastoreServer.FileExists(@"C:\Projects\DFS\dfs.documents\datastore\Z.txt");
        Assert.IsFalse(actual);
    }

    [TestMethod]
    public void GetFileContents_ShouldReturnContentsAsBytes()
    {
        var actual = _datastoreServer.GetFileContents(REAL_FILE).ToArray();
        Assert.IsNotNull(actual);
        Assert.AreEqual(REAL_FILE_SIZE, actual.Length);
    }
}