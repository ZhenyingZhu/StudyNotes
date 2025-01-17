namespace ParentConsoleApp
{
    using ChildLib;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello, World, {MyLib.MyName}");

        }
    }
}
