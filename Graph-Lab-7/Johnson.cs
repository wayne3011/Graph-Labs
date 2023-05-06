using GraphLab;
using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Graph_Lab_5;

namespace Graph_Lab_7
{
    static internal class Johnson
    {
        static public Edge[] Run(Graph graph, out bool negativeEdges)
        {
            negativeEdges = false;
            SortedSet<AdjacentVertex> sourceVertex = new SortedSet<AdjacentVertex>();
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                sourceVertex.Add(new AdjacentVertex(i, 0));
            }
            List<Edge> paths = new List<Edge>();
            Graph graphWithSource = new Graph(graph.AdjacentList.Append(sourceVertex));
            int[] functionH = BellmanFord.Run(graphWithSource, graph.AdjacentList.Length);
            Graph graphForDijkstra = new Graph(graph);
            foreach(var edge in graph.GetListOfEdges())
            {
                if (functionH[edge.vi] != 0 || functionH[edge.vj] != 0)
                {
                    negativeEdges = true;
                }
                graphForDijkstra.AddEdge(edge.vi,edge.vj, edge.weight + functionH[edge.vi] - functionH[edge.vj]);
            }
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                Edge[] pathsDijkstra =  Dijkstra.Run(graphForDijkstra, i);
                for(int j = 0; j < pathsDijkstra.Length;j++)
                {
                    Edge newEdge = new Edge(pathsDijkstra[j].vi, pathsDijkstra[j].vj, pathsDijkstra[j].weight);
                    if (newEdge.vi != newEdge.vj && newEdge.weight != int.MaxValue) paths.Add(newEdge);
                }
            }
            return paths.ToArray();
        }
    }
}
