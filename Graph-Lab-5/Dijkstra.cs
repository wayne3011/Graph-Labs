using GraphLab;
using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_5
{
    static class Dijkstra
    {
        static public Result Run(Graph graph, int beginVertex, int endVertex)
        {
            Result result = new Result();
            Edge[] parents = new Edge[graph.AdjacentList.Length];
            bool[] visited = new bool[graph.AdjacentList.Length];
            int[] markers = new int[graph.AdjacentList.Length];
            for(int i = 0; i < graph.AdjacentList.Length; i++)
            {
                markers[i] = int.MaxValue;
            }
            PriorityQueue<int,int> queue = new PriorityQueue<int,int>();
            markers[beginVertex] = 0;
            queue.Enqueue(beginVertex, 0);
            while(queue.Count > 0)
            {
                int currentVertex = queue.Dequeue();
                if (currentVertex == endVertex) break;
                visited[currentVertex] = true;
                foreach(var adjacentVertex in graph.AdjacentList[currentVertex])
                {
                    if (!visited[adjacentVertex.Vj])
                    {
                        if(markers[adjacentVertex.Vj] > (markers[currentVertex] == int.MaxValue ? int.MaxValue : markers[currentVertex] + adjacentVertex.Weight))
                        {
                            markers[adjacentVertex.Vj] = markers[currentVertex] + adjacentVertex.Weight;
                            parents[adjacentVertex.Vj] = new Edge(currentVertex,adjacentVertex.Vj,adjacentVertex.Weight);
                            queue.Enqueue(adjacentVertex.Vj, markers[adjacentVertex.Vj]);
                        }
                    }
                }
            }
            result.Length = markers[endVertex];
            if (markers[endVertex] == int.MaxValue) return result;
            List<Edge> path = new List<Edge>();
            int vertex = endVertex;
            while(vertex != beginVertex)
            {
                path.Add(parents[vertex]);
                vertex = parents[vertex].vi;
            }
            path.Reverse();
            result.Path = path.ToArray();
            return result;
        }
    }
    struct Result
    {
        public Result() { }
        public Edge[] Path { get; set; } = new Edge[0];
        public int Length { get; set; } = 0;

    }
}
