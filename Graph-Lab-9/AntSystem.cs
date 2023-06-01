using Graph_Lab_9;
using GraphLab;
using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_9
{
    class Pheromones
    {
        private double[][] pheromones;
        public Pheromones(int vertexCount)
        {
            pheromones = new double[vertexCount][];
            for (int i = 0; i < vertexCount; i++)
            {
                pheromones[i] = new double[i + 1];
                for (int j = 0; j < pheromones[i].Length; j++)
                {
                    pheromones[i][j] = 0.2;
                }
            }
        }
        public int GetLength { get { return pheromones.Length; } }
        public double this[int i, int j]
        {
            get
            {
                if (j >= pheromones[i].Length) return pheromones[j][i];
                else return pheromones[i][j];
            }
            set
            {
            if (j >= pheromones[i].Length) pheromones[j][i] = value;
            else pheromones[i][j] = value;
            }
        }
        public void Evaporation(double p)
        {
            for (int i = 0; i < pheromones.Length; i++)
            {
                for (int  j = 0;  j < pheromones[i].Length;  j++)
                {
                    pheromones[i][j] = (1 - p) * pheromones[i][j];
                }
            }
        }
        public PriorityQueue<Edge,double> PheromonePriority 
        { 
            get 
            {
                PriorityQueue<Edge, double> pq = new PriorityQueue<Edge, double>();
                for (int i = 0; i < pheromones.Length; i++)
                {
                    for (int j = 0; j < pheromones[i].Length; j++)
                    {
                        pq.Enqueue(new Edge(i, j), 1/pheromones[i][j]);
                    }
                }
                return pq;
            }
        }
    }
    
    }
    internal class AntSystem
    {
        private Graph graph;
        private double alpha = 1;
        private double beta = 3.5;
        private double q;
        private double p = 0.3;
        private Pheromones pheromones;
        private int beginVertex;
        private int iterationCount = 100;
        private List<Edge> result;
        public AntSystem(Graph graph, int beginVertex)
        {
            this.graph = graph;
            pheromones = new Pheromones(graph.VertexCount);
            result = new List<Edge>();
            this.beginVertex = beginVertex;
        }
        public Edge[] Run()
        {
            for (int iter = 0; iter < iterationCount; iter++)
            {
                List<Ant> ants = new List<Ant>();
                int optimalPath = int.MaxValue;
                for (int i = 0; i < graph.VertexCount; i++)
                {
                    if (i == beginVertex) continue;
                    Ant ant = new Ant(i);
                    AntBypass(ant);
                    if (ant.PathLength < optimalPath) optimalPath = ant.PathLength;
                    ants.Add(ant);
                }
                foreach (var ant in ants)
                {
                    foreach (var edge in ant.Path)
                    {
                        if(edge.vi != edge.vj)pheromones[edge.vi, edge.vj] += (double)optimalPath / (double)ant.PathLength;
                    }
                }
                pheromones.Evaporation(p);
            }
            //GettingResultDFS(beginVertex);
            GettingResultDFS(beginVertex, new bool[graph.VertexCount]);
            return result.ToArray();
        }
        public void GettingResultDFS(int prev, bool[] closed)
        {
            AdjacentVertex maxPheromonesVertex = new AdjacentVertex(0);
            double maxPheromones = int.MinValue;
            foreach (var vertex in graph.AdjacentList[prev])
            {
                if (pheromones[prev, vertex.Vj] > maxPheromones && !closed[vertex.Vj])
                {
                    maxPheromonesVertex = vertex;
                    maxPheromones = pheromones[prev, vertex.Vj];
                }
            }
            closed[maxPheromonesVertex.Vj] = true;
            result.Add(new Edge(prev, maxPheromonesVertex.Vj, maxPheromonesVertex.Weight));
            if (maxPheromonesVertex.Vj != beginVertex) GettingResultDFS(maxPheromonesVertex.Vj, closed);
            else return;
        }
    //public void GettingResultDFS()
    //{

    ////SortedSet<KeyValuePair<double, Edge>> pheromonesSorted = new SortedSet<KeyValuePair<double, Edge>>();
    //var pq = pheromones.PheromonePriority;
    //    while(result.Count != (graph.VertexCount - 1) * 2)
    //    {
    //        result.Add(pq.Dequeue());
    //    }
    //}
    public void AntBypass(Ant ant)
        {
            while (ant.CurrentNode != beginVertex)
            {
                HashSet<KeyValuePair<double, AdjacentVertex>> AttractivenessEachVertexes = new HashSet<KeyValuePair<double, AdjacentVertex>>();
                double TotalAttractiveness = 0;
                foreach (var v in graph.AdjacentList[ant.CurrentNode])
                {
                    if (ant.Closed.Contains(v.Vj)) continue;
                    double attractiveness = Math.Pow(pheromones[ant.CurrentNode, v.Vj],alpha) / Math.Pow(graph.Weight(ant.CurrentNode,v.Vj),beta);
                    KeyValuePair<double, AdjacentVertex> pair = new(attractiveness, v);
                    AttractivenessEachVertexes.Add(pair);
                    TotalAttractiveness += attractiveness;
                }
                double randomProbability = Random.Shared.NextDouble();
                double cumulativeProbability = 0;
                foreach (var nextVertex in AttractivenessEachVertexes)
                {
                    if (randomProbability < cumulativeProbability + nextVertex.Key / TotalAttractiveness)
                    {
                        ant.Closed.Add(nextVertex.Value.Vj);
                        ant.Path.Add(new Edge(ant.CurrentNode, nextVertex.Value.Vj, nextVertex.Value.Weight));
                        ant.CurrentNode = nextVertex.Value.Vj;
                        break;
                    }
                    cumulativeProbability += nextVertex.Key / TotalAttractiveness;
                }
            }
        }

    }
    internal class Ant
    {
        public int CurrentNode { get; set; }
        public List<Edge> Path { get; set; }
        public HashSet<int> Closed { get; set; }
        public int PathLength
        {
            get
            {
                int length = 0;
                foreach (var Edge in Path)
                {
                    length += Edge.weight;
                }
                return length;
            }
        }
        public Ant(int currentNode)
        {
            this.CurrentNode = currentNode;
            Path = new List<Edge>();
            Closed = new HashSet<int>();
        }
    }

