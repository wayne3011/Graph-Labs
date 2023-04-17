namespace GraphLab.Components
{
    internal class Edge
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
            return vi + " " + vj + " " + weight +  "\n";
        }
    }
}
