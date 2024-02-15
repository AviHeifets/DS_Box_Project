using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataStructures
{
    public class LNode<T>
    {
        public T Value { get; set; }
        public LNode<T> Next { get; set; }
        public LNode()
        {
            Value = default!;
            Next = default!;
        }
    }
}
