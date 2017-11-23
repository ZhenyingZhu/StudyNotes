using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class SelectTestClass
    {
        public int Id;

        public bool MightNotTrue;

        public SelectTestClass(int i, bool s)
        {
            Id = i;
            MightNotTrue = s;
        }

        public static SelectTestClass SelectSomeItems(SelectTestClass stc)
        {
            return new SelectTestClass(stc.Id, true);
        }

        public override string ToString()
        {
            return string.Format("id: {0}, selected: {1}", Id, MightNotTrue);
        }
    }
}
