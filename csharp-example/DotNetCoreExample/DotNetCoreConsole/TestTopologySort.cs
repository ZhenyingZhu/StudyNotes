namespace DotNetCoreConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TestTopologySort
    {
        // https://stackoverflow.com/questions/53202300/topological-sort-using-queue-in-a-graph
        public static void TestMain()
        {
            PriorityQueue<string, int> pq = new PriorityQueue<string, int>();
            var node1 = new DependencyNode<string>() { Value = "1" };
            var node2 = new DependencyNode<string>() { Value = "2", Dependencies = new List<DependencyNode<string>>() { node1 } };
            var node3 = new DependencyNode<string>() { Value = "3", Dependencies = new List<DependencyNode<string>>() { node2 } };
            List<DependencyNode<string>> nodes = new ()
            {
                node1,
                node2,
                node3
            };

            var ordered = TopologySort(nodes);
            foreach (var node in ordered)
            {
                Console.WriteLine(node.Value);
            }
        }

        private static List<DependencyNode<string>> TopologySort(List<DependencyNode<string>> nodes)
        {
            var ordered = new List<DependencyNode<string>>();
            var queue = new Queue<DependencyNode<string>>();
            foreach (var node in nodes)
            {
                if (node.Dependencies == null || node.Dependencies.Count == 0)
                {
                    queue.Enqueue(node);
                }
            }

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                ordered.Add(node);
                foreach (var n in nodes)
                {
                    if (n.Dependencies != null && n.Dependencies.Contains(node))
                    {
                        n.Dependencies.Remove(node);
                        if (n.Dependencies.Count == 0)
                        {
                            queue.Enqueue(n);
                        }
                    }
                }
            }

            return ordered;
        }

        private class DependencyNode<T>
        {
            public T Value { get; set; }
            public List<DependencyNode<T>> Dependencies { get; set; }
        }
    }
}
