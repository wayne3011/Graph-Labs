namespace GraphLab.Components
{
    internal class Edge
    {
        int vi = -1;
        int vj = -1;
        int weight = 1;
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
