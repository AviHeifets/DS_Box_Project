using Ds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataStructures
{
    public class Cell
    {
        public LinkList<CustomQueue<Box>> Boxes { get; }

        public Cell()
        {
            Boxes = new LinkList<CustomQueue<Box>>();
        }
    }
}

