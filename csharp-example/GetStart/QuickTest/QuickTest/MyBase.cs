﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    abstract class MyBase
    {
        public int Id { get; set; }

        public virtual void PrintId()
        {
            Console.WriteLine(@"base print Id: {0}", Id);
        }
    }
}
