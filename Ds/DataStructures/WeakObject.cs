using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataStructures
{
    public class WeakObject<T> where T : class
    {

        private WeakReference<T> reference;

        public T GetObject
        {
            get
            {
                T item;
                reference.TryGetTarget(out item!);
                return item!;
            }

        }

        public WeakObject(T obj)
        {
            reference = new WeakReference<T>(obj);

        }

    }
}
