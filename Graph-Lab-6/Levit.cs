using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLab;

namespace Graph_Lab_6
{
    static class Levit
    {
        static public int[] Run(Graph graph, int source)
        {
            int[] markers = new int[graph.VertexCount];
            HashSet<int> rawVertex = new HashSet<int>();
            for (int  i = 0;  i < graph.VertexCount;  i++)
            {
                markers[i] = int.MaxValue;
                rawVertex.Add(i);
            }
            markers[source] = 0;
            HashSet<int> presumablyPreocessed = new HashSet<int>();
            Queue<int> processingQueue = new Queue<int>();
            processingQueue.Enqueue(source);
            while(processingQueue.Count > 0)
            {
                int currentVertex = processingQueue.Dequeue();
                foreach(var adjacentVertex in graph.AdjacentList[currentVertex])
                {
                    if (rawVertex.Contains(adjacentVertex.Vj))
                    {
                        markers[adjacentVertex.Vj] = Math.Min(markers[currentVertex] + adjacentVertex.Weight, markers[adjacentVertex.Vj]);
                        rawVertex.Remove(adjacentVertex.Vj);
                        processingQueue.Enqueue(adjacentVertex.Vj);
                    }
                    else if (presumablyPreocessed.Contains(adjacentVertex.Vj))
                    {
                        if (markers[adjacentVertex.Vj] > markers[currentVertex] + adjacentVertex.Weight)
                        {
                            markers[adjacentVertex.Vj] = markers[currentVertex] + adjacentVertex.Weight;
                            presumablyPreocessed.Remove(adjacentVertex.Vj);
                            processingQueue.Enqueue(adjacentVertex.Vj);
                        }

                    }
                    else if (processingQueue.Contains(adjacentVertex.Vj))
                    {
                        markers[adjacentVertex.Vj] = Math.Min(markers[adjacentVertex.Vj],markers[currentVertex] + adjacentVertex.Weight);
                    };
                }
                presumablyPreocessed.Add(currentVertex);
            }
            return markers;

        }
        
    }
}
