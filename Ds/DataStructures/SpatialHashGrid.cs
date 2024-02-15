using Ds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.DataStructures
{
    public class SpatialHashGrid
    {
        public Cell[,] Grid { get; set; }
        public int GridSize { get; set; }
        public int Count { get; set; } = 0;

        #region Private Props
        private readonly double rangePrecentage;
        private readonly double minimumBoxSize;
        private readonly double maximumBoxSize;
        private double targetBoxMinX;
        private double targetBoxMinY;
        private double targetBoxMaxX;
        private double targetBoxMaxY;
        private int cellX;
        private int cellY;
        private Func<double, double, CustomQueue<Box>>[] functions;
        #endregion

        public SpatialHashGrid(double minBoxSize, double maxBoxSize, double jumpRange, double precentage = 0)
        {
            this.maximumBoxSize = maxBoxSize;
            this.minimumBoxSize = minBoxSize;
            rangePrecentage = 1 + jumpRange;
            this.GridSize = IndexFinder(maxBoxSize) + 1;
            Grid = new Cell[GridSize, GridSize];
            functions = new Func<double, double, CustomQueue<Box>>[] { ExectSearch!, SearchByY!, SearchByX!, NoX_NoY! };
            InitGrid();
        }

        public CustomQueue<Box> Search(double targetX, double targetY)
        {
            targetBoxMinX = targetX * 0.9;
            targetBoxMinY = targetY * 0.9;
            targetBoxMaxX = targetX * 1.1;
            targetBoxMaxY = targetY * 1.1;

            cellX = IndexFinder(targetX);
            cellY = IndexFinder(targetY);
            foreach (var func in functions)
            {
                CustomQueue<Box> queue = func(targetX, targetY);
                if (queue != null)
                {
                    return queue;
                }
            }
            return null!;
        }
        //public void DeleteBox(Box box)
        //{
        //    WeakObject<Box> weak = new WeakObject<Box>(box);
        //    Count--;
        //}
        public void DeleteQueue(CustomQueue<Box> queue) 
        {
            int cellX = IndexFinder(queue.X);
            int cellY = IndexFinder(queue.Y);

            Grid[cellX, cellY].Boxes.Remove(queue);
        }
        public void InsertBox(Box box)
        {
            int cellX = IndexFinder(box.X);
            int cellY = IndexFinder(box.Y);

            CustomQueue<Box> newQueue = new CustomQueue<Box>() { X = box.X, Y = box.Y };
            CustomQueue<Box> queue = Grid[cellX, cellY].Boxes.Search(newQueue);
            if (queue == null)
            {
                Grid[cellX, cellY].Boxes.Add(newQueue);
                newQueue.Items.Enqueue(box);
            }
            else
                queue.Items.Enqueue(box);

            Count++;
        }

        #region Private Functions
        private void InitGrid()
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    Grid[x, y] = new Cell();
                }
            }
        }

        private CustomQueue<Box?>? ExectSearch(double targetX, double targetY)
        {
            if (Grid[cellX, cellY].Boxes.Root == null)
                return null;

            foreach(CustomQueue<Box> queue in Grid[cellX, cellY].Boxes)
            {
                if (queue.X != targetX || queue.Y != targetY)
                    continue;

                if (queue.Items.Count == 0)
                    return null;

                return queue!;
            }
            return null;
        }

        private CustomQueue<Box?>? SearchByX(double targetX, double targetY)
        {
            for (int y = cellY - 7; y <= cellY + 7; y++)
            {
                if (y == cellY)
                    continue;

                if (!IsValidCell(cellX, y) || Grid[cellX, y].Boxes.Root == null)
                    continue;


                if (y < cellY - 6 || y > cellY + 6)
                {
                    foreach (var box in Grid[cellX, y].Boxes)
                    {
                        if (CheackPercentage(box.X, box.Y))
                            return box!;
                    }
                    continue;
                }

                return Grid[cellX, y].Boxes.Root!.Value!;
            }
            return null;
        }

        private CustomQueue<Box?>? SearchByY(double targetX, double targetY)
        {
            for (int x = cellX - 7; x <= cellX + 7; x++)
            {
                if (x == cellX )
                    continue;

                if (!IsValidCell(x, cellY) || Grid[x, cellY].Boxes.Root == null)
                    continue;

                if (x < cellX - 6 || x > cellX + 6 )
                {
                    foreach (var box in Grid[x, cellY].Boxes)
                    {
                        if (CheackPercentage(box.X, box.Y))
                            return box!;
                    }
                    continue;
                }
               
                return Grid[x, cellY].Boxes.Root!.Value!;
            }
            return null;
        }

        //when x and y are not exect
        private CustomQueue<Box>? NoX_NoY(double targetX, double targetY)
        {
            for (int x = cellX - 7; x <= cellX + 7; x++)
            {
                if (x == cellX)
                    continue;

                for (int y = cellY - 7; y <= cellY + 7; y++)
                {
                    if (y == cellY)
                        continue;

                    if (!IsValidCell(x, y) || Grid[x, y].Boxes.Root == null)
                        continue;

                    if (x < cellX - 6 || x > cellX + 6 || y < cellY - 6 || y > cellY + 6)
                    {
                        foreach (var box in Grid[x, y].Boxes)
                        {
                            if (CheackPercentage(box.X, box.Y))
                                return box;
                        }
                        continue;
                    }
                    return Grid[x, y].Boxes.Root!.Value;
                }
            }
            return null!;
        }

        private bool CheackPercentage(double x, double y)
        {
            if (x < targetBoxMinX || x > targetBoxMaxX || y < targetBoxMinY || y > targetBoxMaxY)
                return false;
            return true;
        }

        private bool IsValidCell(int x, int y) => x >= 0 && x < GridSize && y >= 0 && y < GridSize;

        private int IndexFinder(double number)
        {
            double start = minimumBoxSize;
            double jump = rangePrecentage;
            int index = (int)Math.Log(number / start, jump);
            return index;
        }
        #endregion

    }
}
