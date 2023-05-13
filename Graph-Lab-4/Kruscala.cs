using GraphLab.Components;
using GraphLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_4
{
    internal class Kruscala
    {
        static public Edge[] Run(Graph graph)
        {
            Queue<Edge> edges = new Queue<Edge>(graph.GetListOfEdges().OrderBy(c => c.weight));
            List<HashSet<int>> sets = new List<HashSet<int>>();
            List<Edge> result = new List<Edge>();
            while (edges.Count != 0)
            {
                Edge edge = edges.Dequeue();
                int length = sets.Count;
                int firstVertexIndex = -1;
                int secondVertexIndex = -1;
                for (int i = 0; i < length; i++)
                {
                    if (sets[i].Contains(edge.vi)) firstVertexIndex = i;
                    if (sets[i].Contains(edge.vj)) secondVertexIndex = i;
                    if (firstVertexIndex != -1 && secondVertexIndex != -1) break;
                }
                if (firstVertexIndex == secondVertexIndex && (secondVertexIndex != -1)) continue;
                else if (firstVertexIndex == -1 && secondVertexIndex == -1) sets.Add(new HashSet<int> { edge.vi, edge.vj });
                else if (firstVertexIndex == -1 || secondVertexIndex == -1)
                {
                    if (firstVertexIndex == -1)
                    {
                        sets[secondVertexIndex].Add(edge.vi);
                    }
                    else
                    {
                        sets[firstVertexIndex].Add(edge.vj);
                    }
                }
                else
                {
                    HashSet<int> setForJoining = sets[secondVertexIndex];
                    sets[firstVertexIndex] = new HashSet<int>(sets[firstVertexIndex].Union(setForJoining));
                    sets.RemoveAt(secondVertexIndex);
                }
                result.Add(edge);
            }
            return result.ToArray();
        }
    }
}
