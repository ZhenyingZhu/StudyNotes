using DotNetCoreConsole;

namespace DotNetCoreConsole.Unittests;

[TestClass]
public class UriTest
{
    [TestMethod]
    public void TestUriCraft()
    {
        string uri = TestUri.CraftUri();
        Assert.AreEqual(uri, "http://apple/banana/api");
    }
}