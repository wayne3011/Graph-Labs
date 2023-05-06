using GraphLab;
using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_6
{
    internal static class BellmanFord
    {
        public static int[] Run(Graph graph, int beginVertedx)
        {
            Edge[] edgeList = graph.GetListOfEdges();
            int[] markers = new int[graph.AdjacentList.Length];
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                markers[i] = int.MaxValue;
            }
            markers[beginVertedx] = 0;
            for (int i = 0; i < graph.AdjacentList.Length - 1; i++)
            {
                foreach(var edge in edgeList)
                {
                    if (markers[edge.vj] > (markers[edge.vi] == int.MaxValue ? int.MaxValue : markers[edge.vi] + edge.weight))
                    {
                        markers[edge.vj] = markers[edge.vi] + edge.weight; 
                    };
                }
            }
            foreach(var edge in edgeList)
            {
                if (markers[edge.vj] > (markers[edge.vi] == int.MaxValue ? int.MaxValue : markers[edge.vi] + edge.weight)) throw new Exception("Negative Cycle!");
            }
            return markers;
        }
    }
}
