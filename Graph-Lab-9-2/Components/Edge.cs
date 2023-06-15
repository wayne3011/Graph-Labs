namespace GraphLab.Components
{
    public class Edge : IComparable
    {
        public int vi { get; } = -1;
        public int vj { get; } = -1 ;
       
        public int weight { get; } = 1;
        public Edge(int vi, int vj)
        {
            this.vi = vi;
            this.vj = vj;
        }
        public Edge(int vi, int vj, int weight) : this(vi, vj)
        {
            this.weight = weight;
        }
        public override string ToString()
        {
            //return (char)(64+vi+1) + " " + (char)(64+vj+1) + " " + weight +  "\n";
            return (vi) + " " + (vj) + " " + weight +  "\n";
        }
        public int CompareTo(object? obj)
        {
            return weight.CompareTo((obj as Edge)?.weight);
        }

        public override bool Equals(object? obj)
        {
            return obj is Edge edge &&
                   vi == edge.vi &&
                   vj == edge.vj;
        }
    }
}
