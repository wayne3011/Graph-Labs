using GraphLab;
using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLab
{
    internal class ConnectivityStatistics
    {
        private Graph graph;
        public ConnectivityStatistics(Graph graph)
        {
            this.graph = graph;
        }
    
        public int[][] GetConnectivityСomponents()
        {
            List<List<int>> components = new List<List<int>>();
            HashSet<int> unvisitedVertices = new HashSet<int>();
            for (int j = 0; j < graph.AdjacentList.Length; j++)
            {
                unvisitedVertices.Add(j);
            }
            Queue<int> queue = new Queue<int>();
            int i = 0;
            while (unvisitedVertices.Count != 0)
            {
                int unvisitedVertex = unvisitedVertices.First();
                queue.Enqueue(unvisitedVertex);
                unvisitedVertices.Remove(unvisitedVertex);
                components.Add(new List<int>() { unvisitedVertex });
                while (queue.Count != 0)
                {
                    int currentVertex = queue.Dequeue();
                    foreach (var neighboringVertex in graph.AdjacentList[currentVertex])
                    {
                        if (unvisitedVertices.Contains(neighboringVertex.Vj))
                        {
                            unvisitedVertices.Remove(neighboringVertex.Vj);
                            queue.Enqueue(neighboringVertex.Vj);
                            components[i].Add(neighboringVertex.Vj);
                        }
                    }
                }
                i++;
            }
            int[][] result = new int[components.Count][];
            for (int k = 0; k < components.Count; k++)
            {
                result[k] = components[k].ToArray();
            }
            return result;

        }
        
        public Graph Inverse()
        {
            Graph inverseGraph = new Graph(graph.AdjacentList.Length);
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                foreach (var adjacentVertex in graph.AdjacentList[i])
                {
                    inverseGraph.AdjacentList[adjacentVertex.Vj].Add(new AdjacentVertex(i, adjacentVertex.Weight));
                }
            }
            return inverseGraph;
        }
        private int ClockDfs(int currentVertex, int[] vertices, ref int time, Graph graph)
        {
            vertices[currentVertex] = time;
            foreach (var adjacentVertex in graph.AdjacentList[currentVertex])
            {
                if (vertices[adjacentVertex.Vj] == 0) ClockDfs(adjacentVertex.Vj, vertices, ref time, graph);
            }
            vertices[currentVertex] = time;
            time++;
            return time;
        }
        private void DfsKosarayu(List<int> component, int vertex, int[] vertices)
        {
            vertices[vertex] = int.MinValue;
            foreach (var adjacentVertex in graph.AdjacentList[vertex])
            {
                if (vertices[adjacentVertex.Vj] != int.MinValue) DfsKosarayu(component, adjacentVertex.Vj, vertices);
            }
            component.Add(vertex);
        }
        public int[][] Kosarayu()
        {
            Graph graph = Inverse();
            int[] vertices = new int[graph.AdjacentList.Length];
            int time = 1;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i] == 0) ClockDfs(i, vertices, ref time, graph);
            }


            List<List<int>> components = new List<List<int>>();
            int k = 0;
            int newVertex = -1;
            while ((newVertex = vertices.FirstMax()) >= 0)
            {
                components.Add(new List<int>());
                DfsKosarayu(components[k], newVertex, vertices);
                k++;
            }
            int[][] result = new int[components.Count][];
            for (int i = 0; i < components.Count; i++)
            {
                result[i] = components[i].ToArray();
            }
            return result;


        }
    }
    internal static class Extexsions
    {
        public static int FirstMax(this int[] array)
        {
            int indexOfMax = -1;
            int maxValue = int.MinValue;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > maxValue)
                {
                    maxValue = array[i];
                    indexOfMax = i;
                }
            }
            return indexOfMax;

        }
    }
}
