using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Models
{
    public class Box
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Count { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }

        public double Base
        {
            get
            {
                return X * X;
            }
        }

        public Box()
        {
            ExpiryDate = DateTime.Now.AddDays(30);
        }
        public override string ToString()
        {
            return $"base: {X * X} , height: {Y}";
        }


    }
}
