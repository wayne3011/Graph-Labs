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
        static public Edge[] Run(Graph graph, int beginVertex)
        {
            Edge[] parents = new Edge[graph.AdjacentList.Length];
            bool[] visited = new bool[graph.AdjacentList.Length];
            Edge[] markers = new Edge[graph.AdjacentList.Length];
            int[] numberOfVertexVisits = new int[graph.AdjacentList.Length];
            for(int i = 0; i < graph.AdjacentList.Length; i++)
            {
                markers[i] = new Edge(beginVertex,i,int.MaxValue);
            }
            PriorityQueue<int,int> queue = new PriorityQueue<int,int>();
            markers[beginVertex] = new Edge(beginVertex,beginVertex,0);
            queue.Enqueue(beginVertex, 0);
            while(queue.Count > 0)
            {
                int currentVertex = queue.Dequeue();
                visited[currentVertex] = true;
                numberOfVertexVisits[currentVertex]++;
                if (numberOfVertexVisits[currentVertex] == graph.AdjacentList.Length) throw new Exception("Negative cycle!");
                foreach(var adjacentVertex in graph.AdjacentList[currentVertex])
                {
                    
                    if(markers[adjacentVertex.Vj].weight > (markers[currentVertex].weight ==  int.MaxValue ? int.MaxValue : markers[currentVertex].weight + adjacentVertex.Weight))
                    {
                        if(visited[adjacentVertex.Vj]) visited[adjacentVertex.Vj] = false;
                        markers[adjacentVertex.Vj] = new Edge(beginVertex,adjacentVertex.Vj,markers[currentVertex].weight + adjacentVertex.Weight);
                        parents[adjacentVertex.Vj] = new Edge(currentVertex,adjacentVertex.Vj,adjacentVertex.Weight);
                        queue.Enqueue(adjacentVertex.Vj, markers[adjacentVertex.Vj].weight);
                    }
                    
                }
            }
            
            return markers;
        }
    }
}
