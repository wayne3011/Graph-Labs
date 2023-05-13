using Graph_Lab3.Components.OutputModels;
using GraphLab.Components;
using GraphLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab3
{
    internal class BridgeAndHingesSearch
    {
        static private void BridgesAndHingesDfs(int currentVertex, int parentVertex, int[] tin, int[] tup, ref int time, bool[] used, List<Edge> briges, 
            HashSet<int> hinges, ref int firstVertexCounter, ref int firstVertex, Graph graph)
        {
            tup[currentVertex] = tin[currentVertex] = time;
            time++;
            used[currentVertex] = true;
            foreach (AdjacentVertex adjacentVertex in graph.AdjacentList[currentVertex])
            {
                if (adjacentVertex.Vj == parentVertex) continue;
                else if (used[adjacentVertex.Vj]) tup[currentVertex] = Math.Min(tup[currentVertex], tin[adjacentVertex.Vj]);
                else
                {
                    BridgesAndHingesDfs(adjacentVertex.Vj, currentVertex, tin, tup, ref time, used, briges, hinges, ref firstVertexCounter, ref firstVertex,graph);
                    tup[currentVertex] = Math.Min(tup[currentVertex], tup[adjacentVertex.Vj]);
                    if (tup[adjacentVertex.Vj] > tin[currentVertex]) briges.Add(new Edge(currentVertex, adjacentVertex.Vj));
                    if (tup[adjacentVertex.Vj] >= tin[currentVertex])
                    {
                        if (currentVertex != firstVertex || firstVertexCounter == 1) hinges.Add(currentVertex);
                        else firstVertexCounter++;
                    }
                }
            }
        }
        static public BridgesAndHingesResult Run(Graph graph)
        {
            bool[] used = new bool[graph.AdjacentList.Length];
            int[] tin = new int[graph.AdjacentList.Length];
            int[] tup = new int[graph.AdjacentList.Length];
            List<Edge> bridges = new List<Edge>();
            HashSet<int> hinges = new HashSet<int>();
            BridgesAndHingesResult result = new BridgesAndHingesResult();
            int time = 1;

            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                int firstVertexCounter = 0;
                if (used[i] == false) BridgesAndHingesDfs(i, -1, tin, tup, ref time, used, bridges, hinges, ref firstVertexCounter, ref i,graph);
            }
            result.Bridges = bridges.ToArray();
            result.Hinges = hinges.ToArray();
            return result;
        }
    }
}
