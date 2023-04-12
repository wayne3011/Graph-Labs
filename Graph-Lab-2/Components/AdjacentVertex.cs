namespace GraphLab.Components
{
    public class AdjacentVertex : IComparable
    {
        public int Vj;
        public int Weight;
        public bool Mark = false;
        public AdjacentVertex(int vj)
        {
            Vj = vj;
        }
        public AdjacentVertex(int vj, int weight) : this(vj)
        {
            Weight = weight;
        }
        public AdjacentVertex(int vj, int weight, bool mark) : this(vj, weight)
        {
            Mark = mark;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            AdjacentVertex? otherAdjacentVertex = obj as AdjacentVertex;
            if (otherAdjacentVertex != null) return Vj.CompareTo(otherAdjacentVertex.Vj);
            else throw new ArgumentException("Object is not AdjacentVertex");
            
        }

        public static bool operator==(AdjacentVertex? obj1, AdjacentVertex? obj2)
        {
            if(obj1 is null && obj2 is null) return true;
            if(obj1 is null || obj2 is null) return false;
            return obj1.Vj == obj2.Vj;
        }
        public static bool operator !=(AdjacentVertex? obj1, AdjacentVertex? obj2)
        {
            if (obj1 is null && obj2 is null) return false;
            if (obj1 is null || obj2 is null) return true;
            return obj1.Vj != obj2.Vj;
        }
        //public static bool operator <(AdjacentVertex obj1, AdjacentVertex obj2)
        //{
        //    return obj1.Vj < obj2.Vj;
        //}
        //public static bool operator >(AdjacentVertex obj1,AdjacentVertex obj2)
        //{
        //    return obj1.Vj > obj2.Vj;
        //}

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return this.Vj.ToString();
        }
    }
}
