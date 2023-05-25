using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLab;
using GraphLab.Components;

namespace Graph_Lab_10
{
    internal class FordFulkerson
    {
        private readonly Graph graph;
        public int S;
        public int T;
        public Edge[] GetStream()
        { 
             
            
                List<Edge> edges = new List<Edge>();
                for (int vi = 0; vi < graph.AdjacentList.Length; vi++)
                {
                    foreach (var vertex in graph.AdjacentList[vi])
                    {
                        Edge edge = new Edge(vi,vertex.Vj,stream.Weight(vi,vertex.Vj));
                        edges.Add(edge);
                    }
                }
                return edges.ToArray();
                
            
        }
        private Graph stream;
        public FordFulkerson(Graph graph)
        {
            this.graph = graph;
            stream = new Graph(graph.VertexCount);
            StaticsCollector staticsCollector = new StaticsCollector(graph);
            DegreeVector degreeVector = staticsCollector.GetDegreeVector();
            S = Array.IndexOf(degreeVector.GetIncomingVector(), 0);
            T = Array.IndexOf(degreeVector.GetOutgoingVector(), 0);
            foreach (var edge in graph.GetListOfEdges())
            {
                stream.AddEdge(edge.vi, edge.vj, 0);
            }
        }
        public int Run()
        {
            int StreamPower = 0;
            while (true)
            {
                bool[] mark = new bool[graph.AdjacentList.Length];
                int delta = DFS(S, int.MaxValue, mark);
                if (delta > 0) StreamPower += delta;
                else return StreamPower;
            }
        }
        public int DFS(int v, int delta, bool[] mark)
        {
            if (mark[v]) return 0;
            mark[v] = true;
            if (v == T) return delta;
            foreach(var adjacencyVertex in stream.AdjacentList[v])
            {
                int Fvu = stream.Weight(v, adjacencyVertex.Vj);
                int Cvu = graph.Weight(v, adjacencyVertex.Vj);
                if (Fvu < Cvu)
                {
                    int tmpDelta = DFS(adjacencyVertex.Vj, Math.Min(delta,Cvu - Fvu), mark);
                    if (tmpDelta > 0)
                    {
                        //if (Cvu - tmpDelta <= 0) stream.DeleteEdge(adjacencyVertex.Vj, v);
                        //else 
                        //if (Cvu == Fvu + tmpDelta) graph.DeleteEdge(v, adjacencyVertex.Vj);
                        //else 
                        stream.AddEdge(v, adjacencyVertex.Vj, Fvu + tmpDelta);
                        stream.AddEdge(adjacencyVertex.Vj, v, stream.Weight(adjacencyVertex.Vj,v) - tmpDelta);
                        return tmpDelta;
                    }
                }
            }
            return 0;

        }

    }
}
