using DotNetCoreConsole;

namespace DotNetCoreConsole.Unittests;

[TestClass]
public class MultiThreadingTest
{
    [TestMethod]
    [ExpectedException(typeof(AggregateException))]
    public void TestUnwrapTaskWithTimeout()
    {
        TaskCompletionSource<int> testTaskSource = new TaskCompletionSource<int>();
        Task<Task<int>> wrappedTestTask = Task<int>.FromResult(testTaskSource.Task);

        MultiThreading.UnwrapTaskWithTimeout<int>(wrappedTestTask, TimeSpan.Zero).Wait();
    }

    [TestMethod]
    public async Task UnwrapTaskWithTimeout_Must_ReturnResultIfTaskCompletes()
    {
        const int TestNumber = 123;
        Task<Task<int>> wrappedTestTask = Task<int>.FromResult(Task.FromResult(TestNumber));

        int result = await MultiThreading.UnwrapTaskWithTimeout<int>(wrappedTestTask, TimeSpan.FromSeconds(1));

        Assert.AreEqual(TestNumber, result);
    }
}