using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhenyingTest
{
    abstract class Parent
    {
        public int Id { get; set; }
    }

    class Child : Parent
    {
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Child child = new Child()
            {
                Id = 1,
                Name = "child"
            };

            Console.WriteLine("See if it works: " + child.Name + child.Id);
            
            Console.ReadLine();
        }

    }
}
