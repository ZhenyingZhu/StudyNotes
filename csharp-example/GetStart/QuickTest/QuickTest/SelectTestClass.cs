using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class SelectTestClass
    {
        private readonly int _id;

        private readonly bool _mightNotTrue;

        public SelectTestClass(int i, bool s)
        {
            _id = i;
            _mightNotTrue = s;
        }

        public static SelectTestClass SelectSomeItems(SelectTestClass stc)
        {
            return new SelectTestClass(stc._id, true);
        }

        public override string ToString()
        {
            return string.Format("id: {0}, selected: {1}", _id, _mightNotTrue);
        }
    }
}
