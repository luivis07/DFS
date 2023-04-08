using dfs.core.common.helpers;
namespace dfs.core.common.tests;

[TestClass]
public class PathProviderTests
{
    [TestMethod]
    public void GetSolutionBasePath_ReturnsExpectedValue()
    {
        var basePath = PathProvider.GetSolutionBasePath();
        Assert.AreEqual(@"C:\Projects\DFS", basePath, true);
    }

    [TestMethod]
    public async Task GetDatastoreBasePath_ReturnsExpectedValue()
    {
        var datastorePath = await PathProvider.GetDatastoreBasePath();
        Assert.AreEqual(@"C:\Projects\DFS\dfs.documents\datastore", datastorePath, true);
    }
}