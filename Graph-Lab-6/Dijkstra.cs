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
        static public int[] Run(Graph graph, int beginVertex)
        {
            Edge[] parents = new Edge[graph.AdjacentList.Length];
            bool[] visited = new bool[graph.AdjacentList.Length];
            int[] markers = new int[graph.AdjacentList.Length];
            int[] numberOfVertexVisits = new int[graph.AdjacentList.Length];
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
                visited[currentVertex] = true;
                numberOfVertexVisits[currentVertex]++;
                if (numberOfVertexVisits[currentVertex] == graph.AdjacentList.Length) throw new Exception("Negative cycle!");
                foreach(var adjacentVertex in graph.AdjacentList[currentVertex])
                {
                    
                    if(markers[adjacentVertex.Vj] > (markers[currentVertex] == int.MaxValue ? int.MaxValue : markers[currentVertex] + adjacentVertex.Weight))
                    {
                        if(visited[adjacentVertex.Vj]) visited[adjacentVertex.Vj] = false;
                        markers[adjacentVertex.Vj] = markers[currentVertex] + adjacentVertex.Weight;
                        parents[adjacentVertex.Vj] = new Edge(currentVertex,adjacentVertex.Vj,adjacentVertex.Weight);
                        queue.Enqueue(adjacentVertex.Vj, markers[adjacentVertex.Vj]);
                    }
                    
                }
            }
            
            return markers;
        }
    }
}
