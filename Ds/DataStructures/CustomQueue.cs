namespace Ds.DataStructures
{
    public class CustomQueue<T>
    {
        public Queue<T> Items { get; set; }
        public int Amount { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public CustomQueue()
        {
            Items = new Queue<T>();
            Amount = 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) 
                return false;

            if (obj is not CustomQueue<T>)
                return false;

            CustomQueue<T>? other = obj as CustomQueue<T>;

            if (this.X != other!.X || this.Y != other.Y) 
                return false;

            return true;
        }

    }
}