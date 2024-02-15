using Ds.DataStructures;
using Ds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds
{
    public class DataService
    {
        public static DataService Init { get; } = new DataService();
        private DataService()
        {
            grid = new SpatialHashGrid(1, 1000, 0.015);
            allBoxes = new LinkList<Box>();
            Expired = new List<Box>();
        }

        private SpatialHashGrid? grid;
        private LinkList<Box> allBoxes;
        private List<Box> Expired;

        public int Count() => grid!.Count;
        public void ClearExpired() => Expired.Clear();

        public void CheckExpiration()
        {
            DateTime now = DateTime.Now;
            for (int i = 0; i < grid!.GridSize; i++)
            {
                for (int j = 0; j < grid.GridSize; j++)
                {
                    if (grid.Grid[i, j].Boxes == null)
                        continue;

                    foreach (var queue in grid.Grid[i, j].Boxes)
                    {

                        while (queue.Items.Count != 0 && queue.Items.Peek().ExpiryDate < now)
                        {
                            Box box = queue.Items.Dequeue();
                            box.IsExpired = true;
                            Expired.Add(box);
                        }
                    }
                }
            }
        }

        public CustomQueue<Box> Search(double x, double y) => grid!.Search(x, y);
        public void Add(Box box)
        {
            grid!.InsertBox(box);
            allBoxes.Add(box);
        }

        public List<Box> Buy(CustomQueue<Box> boxes, int amount)
        {
            List<Box> Removed = new List<Box>();

            if (boxes == null || boxes.Items.Count == 0)
                return null;

            int removed = 0;
            while (removed < amount)
            {
                if (boxes.Items.Peek().Count <= amount - removed)
                {
                    removed += boxes.Items.Peek().Count;
                    Removed.Add(boxes.Items.Dequeue());
                    continue;
                }

                boxes.Items.Peek().Count -= amount - removed;
                break;
            }

            if (boxes.Items.Count == 0)
                Delete(boxes);

            return Removed;
        }

        public void Delete(CustomQueue<Box> queue) => grid!.DeleteQueue(queue);
        public List<Box> GetAllBoxes(out List<Box> expired)
        {
            CheckExpiration();
            expired = Expired;
            List<Box> boxes = new List<Box>();
            foreach (var box in allBoxes)
            {
                if (box == null || box.IsExpired == true) continue;
                boxes.Add(box);
            }
            return boxes;
        }

    }
}
