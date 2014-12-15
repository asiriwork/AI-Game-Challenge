using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Test1.AI
{
    public class NodeComparer : IComparer
    {
        public NodeComparer()
        {

        }

        public int Compare(object x, object y)
        {
            return ((Node)x).totalCost - ((Node)y).totalCost;
        }
    }
}
