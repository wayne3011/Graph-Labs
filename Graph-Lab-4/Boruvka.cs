using GraphLab.Components;
using GraphLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_4
{
    internal class Boruvka
    {
        static public int[] GetBoruvkaComponents(Graph graph, out int componentsCount)
        {
            //List<List<int>> components = new List<List<int>>();
            int[] components = new int[graph.AdjacentList.Length];
            bool[] visited = new bool[graph.AdjacentList.Length];
            int j = 0;
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                if (!visited[i])
                {
                    GetBoruvkaComponentsDFS(graph, j, visited, i, components);
                    j++;
                }
            }
            componentsCount = j;
            return components;
        }
        static private void GetBoruvkaComponentsDFS(Graph graph, int currentComponent, bool[] visited, int currentVertex, int[] components)
        {
            visited[currentVertex] = true;
            components[currentVertex] = currentComponent;
            foreach (var adjacentVertex in graph.AdjacentList[currentVertex])
            {
                if (!visited[adjacentVertex.Vj]) GetBoruvkaComponentsDFS(graph,currentComponent, visited, adjacentVertex.Vj, components);
            }
        }
        static public Edge[] Run(Graph graph)
        {
            //List<Edge> tree = new List<Edge>();
            Graph tree = new Graph(graph.AdjacentList.Length);
            //List<List<int>> components = new List<List<int>>();
            int[] components;
            Edge[] edges = graph.GetListOfEdges();
            while (tree.EdgeCount != graph.AdjacentList.Length - 1)
            {
                int minEdgeSize;
                components = GetBoruvkaComponents(tree,out minEdgeSize);
                Edge[] minEdge = new Edge[minEdgeSize];//?? MAP
                for (int i = 0; i < minEdge.Length; i++)
                {
                    minEdge[i] = new Edge(0, 0, int.MaxValue);
                }
                for (int i = 0; i < edges.Length; i++)
                {
                    if (components[edges[i].vi] != components[edges[i].vj])
                    {
                        if (minEdge[components[edges[i].vi]].weight > edges[i].weight)
                        {
                            minEdge[components[edges[i].vi]] = edges[i];
                        }
                        if (minEdge[components[edges[i].vj]].weight > edges[i].weight)
                        {
                            minEdge[components[edges[i].vj]] = edges[i];
                        }
                    }
                }
                for (int i = 0; i < minEdge.Length; i++)
                {
                    if (minEdge[i].weight != int.MaxValue) tree.AddEdge(minEdge[i]);
                    if (minEdge[i].weight != int.MaxValue) tree.AddEdge(minEdge[i]);
                }

            }
            return tree.GetListOfEdges();
        }
    }
}
