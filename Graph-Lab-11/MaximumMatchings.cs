using GraphLab;
using GraphLab.Components;
using Graph_Lab_10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_11
{
    internal class MaximumMatchings
    {
        private readonly Graph graph;
        public MaximumMatchings(Graph graph)
        {
            if (graph.IsDirected) graph = CorrelatedGraph(graph);
            this.graph = graph;
        }
        public Edge[] Run()
        {
            (int[] firstShare, int[] secondShare) = CheckBipartite();
            Graph processedGraph = new Graph(graph.VertexCount);
            foreach(var edge in graph.GetListOfEdges())
            {
                if (firstShare.Contains(edge.vi) && secondShare.Contains(edge.vj)) processedGraph.AddEdge(edge);
                    
            }
            int s = processedGraph.AddVertex();
            for (int i = 0; i < firstShare.Length; i++)
            {
                processedGraph.AddEdge(s, firstShare[i], 1);
            }
            int t = processedGraph.AddVertex();
            for (int i = 0; i < secondShare.Length; i++)
            {
                processedGraph.AddEdge(secondShare[i], t, 1);
            }
            FordFulkerson ff = new FordFulkerson(processedGraph);
            ff.Run();
            List<Edge> matchings = new List<Edge>();
            foreach(var edge in ff.GetStream())
            {
                if (edge.vi == s || edge.vj == t || edge.weight == 0) continue;
                matchings.Add(edge);
                
            }
            return matchings.ToArray();
        }
        public (int[], int[]) CheckBipartite()
        {
            char[] colors = new char[graph.AdjacentList.Length];
            List<int> firstShare = new List<int>();
            List<int> secondShare = new List<int>();
            DFS(0,(char)1,colors);
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i] == 1) firstShare.Add(i);
                else if (colors[i] == 2) secondShare.Add(i);
            }
            return (firstShare.ToArray(), secondShare.ToArray());
        }
        private void DFS(int vertex, char color, char[] colors)
        {
            colors[vertex] = color;
            foreach(var adjacent in graph.AdjacentList[vertex]) 
            {
                if (colors[adjacent.Vj] == 0) DFS(adjacent.Vj, (char)(color == 1 ? 2 : 1), colors);
                else if (colors[adjacent.Vj] == color) throw new Exception("Graph is not bipartite");
            }
        }
        static Graph CorrelatedGraph(Graph graph)
        {
            Graph correlatedGraph = new Graph(graph);
            if (!graph.IsDirected) return correlatedGraph;
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                foreach (var adjacentVertex in graph.AdjacentList[i])
                {
                    correlatedGraph.AdjacentList[adjacentVertex.Vj].Add(new AdjacentVertex(i, 1));
                }

            }
            correlatedGraph.CheckDirected();
            return correlatedGraph;
        }
    }
}
