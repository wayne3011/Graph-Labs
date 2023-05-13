using GraphLab.Components;
using GraphLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_4
{
    internal class Prima
    {
        static public Edge[] Run(Graph graph)
        {
            List<Edge> tree = new List<Edge>();
            HashSet<int> visited = new HashSet<int>();
            bool[] used = new bool[graph.AdjacentList.Length];
            visited.Add(0);
            used[0] = true;
            while (tree.Count != graph.AdjacentList.Length - 1)
            {
                int minWeightEdge = int.MaxValue;
                int minWeightVertexNumber = -1;
                int minWeigthVertexParent = -1;
                foreach (var vertex in visited)
                {
                    foreach (var adjacentVertex in graph.AdjacentList[vertex])
                    {
                        if (!used[adjacentVertex.Vj] && adjacentVertex.Weight < minWeightEdge)
                        {
                            minWeightEdge = adjacentVertex.Weight;
                            minWeightVertexNumber = adjacentVertex.Vj;
                            minWeigthVertexParent = vertex;
                        }
                    }
                }
                used[minWeightVertexNumber] = true;
                tree.Add(new Edge(minWeigthVertexParent, minWeightVertexNumber, graph.Weight(minWeigthVertexParent, minWeightVertexNumber)));
                visited.Add(minWeightVertexNumber);

            }
            return tree.ToArray();
        }
    }
}
