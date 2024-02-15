using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataStructures
{
    public class LinkList<T> : IEnumerable<T>
    {
        public LNode<T> ?Root { get; set; }
        private int count;

        public LinkList()
        {
            Root = null;
            count = 0;
        }
        public void Add(T item)
        {
            LNode<T> newNode = new LNode<T>();
            newNode.Value = item;
            newNode.Next = Root!;
            Root = newNode;
            count++;
        }

        public void Remove(T item) 
        {
            LNode<T> cursor = Root!;

            if (cursor == null) return;

            if (cursor.Value!.Equals(item))
            {
                cursor.Value = default!;
                cursor = cursor.Next;
                Root = cursor;
                count--;
                return;
            }

            while (cursor.Next != null)
            {
                if (cursor.Next.Value!.Equals(item))
                {
                    cursor.Next.Value = default!;
                    cursor.Next = cursor.Next.Next;
                    count--;
                    return;
                }
                   
            }
        }

        public T Search(T item)
        {
            LNode<T> cursor = Root!;
            if (cursor == null) return default!;
            do
            {
                if (cursor.Value!.Equals(item))
                    return cursor.Value;
                cursor = cursor.Next;
            }
            while (cursor.Next != null);
           
            return default!;
        }

        public IEnumerator<T> GetEnumerator()
        {
            LNode<T> cursor = Root!;
           
            while (cursor != null)
            {
                yield return cursor.Value;

                cursor = cursor.Next;
            }
    
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count => count;
    }
}
