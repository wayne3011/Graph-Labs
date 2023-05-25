using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GraphLab.Components;
using System.Threading.Tasks;

namespace GraphLab
{
    internal class Graph
    {
        private readonly SortedSet<AdjacentVertex>[] _adjacentList;
        public SortedSet<AdjacentVertex>[] AdjacentList { get { return _adjacentList; } }
        public bool IsDirected { get; }
        private int _edgeCount;
        public int EdgeCount { get { return _edgeCount; } }
        public int VertexCount { get { return _adjacentList.Length; } }
        public Graph()
        {
            _adjacentList = new SortedSet<AdjacentVertex>[0];
            IsDirected = false;
        }
        public Graph(int vertexCount)
        {
            IsDirected = false;
            _adjacentList = new SortedSet<AdjacentVertex>[vertexCount];
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                _adjacentList[i] = new SortedSet<AdjacentVertex>();
            }
        }
        public Graph(string filePath, InputFileType fileType)
        {

            switch (fileType)
            {
                case InputFileType.EdgesList:
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        Dictionary<int, SortedSet<AdjacentVertex>> adjacentList = new Dictionary<int, SortedSet<AdjacentVertex>>();
                        string Edges = reader.ReadToEnd() ?? string.Empty;
                        foreach (var line in Edges.Split('\n'))
                        {
                            if (line == string.Empty) continue;
                            string[] values = line.Trim(' ').Split(' ');
                            int vi = int.Parse(values[0]) - 1;
                            int vj = int.Parse(values[1]) - 1;
                            int weight = values.Length == 2 ? 1 : int.Parse(values[2]);
                            if (!adjacentList.ContainsKey(vi))
                            {
                                adjacentList.Add(vi, new SortedSet<AdjacentVertex>());
                            }
                            adjacentList[vi].Add(new AdjacentVertex(vj, weight));
                        }
                        int vertexCount = adjacentList.Keys.Max() + 1;
                        _adjacentList = new SortedSet<AdjacentVertex>[vertexCount];

                        for (int i = 0; i < vertexCount; i++)
                        {
                            _adjacentList[i] = new SortedSet<AdjacentVertex>();
                        }
                        foreach (var pair in adjacentList)
                        {
                            _adjacentList[pair.Key] = pair.Value;
                        }
                    }
                    break;
                case InputFileType.AdjacencyMatrix:
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string matrix = reader.ReadToEnd() ?? string.Empty;
                        int i = 0;
                        List<SortedSet<AdjacentVertex>> adjacentList = new List<SortedSet<AdjacentVertex>>();
                        foreach (var line in matrix.Split("\n"))
                        {
                            int j = 0;
                            SortedSet<AdjacentVertex> adjacentVerticeSet = new SortedSet<AdjacentVertex>();
                            if (line == string.Empty) continue;
                            foreach (var number in Regex.Replace(line, ",", String.Empty).Trim('\r', ' ').Split(' '))
                            {
                                int weight = Int32.Parse(number);
                                if (weight != 0)
                                {
                                    adjacentVerticeSet.Add(new AdjacentVertex(j, weight));
                                    _edgeCount++;
                                }
                                j++;

                            }
                            adjacentList.Add(adjacentVerticeSet);
                            i++;
                        }
                        _adjacentList = adjacentList.ToArray();
                    }
                    break;
                case InputFileType.AdjacencyList:
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        List<SortedSet<AdjacentVertex>> adjacentList = new List<SortedSet<AdjacentVertex>>();
                        string AdjacentVertices = reader.ReadToEnd() ?? string.Empty;
                        AdjacentVertices = AdjacentVertices.Remove(AdjacentVertices.Length - 1);
                        int i = 0;
                        foreach (var line in AdjacentVertices.Split('\n'))
                        {
                            if (line == string.Empty)
                            {
                                adjacentList.Add(new SortedSet<AdjacentVertex>());
                                continue;
                            };
                            string[] values = line.Trim(' ').Split(' ');
                            adjacentList.Add(new SortedSet<AdjacentVertex>());
                            foreach (var value in values)
                            {
                                adjacentList[i].Add(new AdjacentVertex(int.Parse(value) - 1, 1));
                            }
                            i++;
                        }
                        _adjacentList = adjacentList.ToArray();
                    }
                    break;
                default:
                    _adjacentList = new SortedSet<AdjacentVertex>[0];
                    break;
            }

            IsDirected = CheckDirected();
        }
        public Graph(Graph graph)
        {
            IsDirected = graph.IsDirected;
            _adjacentList = new SortedSet<AdjacentVertex>[graph._adjacentList.Length];
            _edgeCount = graph._edgeCount;
            for (int i = 0; i < graph._adjacentList.Length; i++)
            {
                _adjacentList[i] = new SortedSet<AdjacentVertex>(graph._adjacentList[i]);
            }
        }
        /// <summary>
        /// Return the adjacency matrix of the Graph
        /// </summary>
        /// <returns>Two-dimensional array</returns>
        public int[][] GetAdjacencyMatrix()
        {
            int[][] matrix = new int[_adjacentList.Length][];
            int i = 0;
            foreach (var vertex in _adjacentList)
            {
                matrix[i] = new int[_adjacentList.Length];
                foreach (var element in _adjacentList[i])
                {
                    matrix[i][element.Vj] = element.Weight;
                }
                i++;
            }
            return matrix;
        }
        /// <summary>
        /// Return the adjacency matrix of the Graph
        /// </summary>
        /// <returns>Return the adjacency matrix, where is there no path, int.MaxValue is used</returns>
        private int[][] GetAdjacencyMatrixForWarshall()
        {
            int[][] matrix = new int[_adjacentList.Length][];
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                matrix[i] = new int[_adjacentList.Length];
                for (int j = 0; j < _adjacentList.Length; j++)
                {
                    if (i == j) matrix[i][j] = 0;
                    else
                    {
                        AdjacentVertex vertex = _adjacentList[i].FirstOrDefault(v => v.Vj == j, new AdjacentVertex(j, int.MaxValue));
                        matrix[i][j] = vertex.Weight;
                    }

                }
            }
            return matrix;
        }
        /// <summary>
        /// Return the weight of Edge
        /// </summary>
        public int Weight(int vi, int vj)
        {
            return _adjacentList[vi].FirstOrDefault(vertex => vertex.Vj == vj, new AdjacentVertex(0, int.MaxValue)).Weight;
        }
        /// <summary>
        /// Check for an edge
        /// </summary>
        public bool IsEdge(int vi, int vj)
        {
            return !(_adjacentList[vi].FirstOrDefault(vertex => vertex?.Vj == vj, null) is null);
        }
        /// <summary>
        /// Returns the list of adjacent vertices
        /// </summary>
        public int[] GetAdjacencyList(int vertex)
        {
            int[] result = new int[_adjacentList[vertex].Count];
            int i = 0;
            foreach (var adjacentVertex in _adjacentList[vertex])
            {
                result[i] = adjacentVertex.Vj;
                i++;
            }
            return result;
        }
        /// <summary>
        /// Returns the list of Graph Edges
        /// </summary>
        public Edge[] GetListOfEdges()
        {
            List<Edge> listOfEdges = new List<Edge>();
            int i = 0;
            foreach (var adjacentVertices in _adjacentList)
            {
                foreach (var vertex in adjacentVertices)
                {
                    listOfEdges.Add(new Edge(i, vertex.Vj, vertex.Weight));
                }
                i++;
            }
            return listOfEdges.ToArray();
        }
        /// <summary>
        /// Returns the list of Edges incident to vertex/coming from the vertex 
        /// </summary>
        public Edge[] GetListOfEdges(int vertex)
        {
            List<Edge> listOfEdges = new List<Edge>();
            foreach (var adjacentVertex in _adjacentList[vertex])
            {
                listOfEdges.Add(new Edge(vertex, adjacentVertex.Vj, adjacentVertex.Weight));
            }
            return listOfEdges.ToArray();
        }
        /// <returns>True if Graph is directed</returns>
        private bool CheckDirected()
        {
            bool directed = false;
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                foreach (var adjacentVertex in _adjacentList[i])
                {
                    if (adjacentVertex.Weight != _adjacentList[adjacentVertex.Vj].FirstOrDefault(vertex => vertex.Vj == i, new AdjacentVertex(0, 0)).Weight)
                    {
                        directed = true;
                        break;
                    };
                    if (directed) break;
                }
            }
            return directed;
        }
        public void AddEdge(int vi, int vj, int weight)
        {
            if (!_adjacentList[vi].Contains(new AdjacentVertex(vj, 0))) _edgeCount++;
            else _adjacentList[vi].Remove(new AdjacentVertex(vj, 0));
            _adjacentList[vi].Add(new AdjacentVertex(vj, weight));
            //if (!IsDirected) _adjacentList[vj].Add(new AdjacentVertex(vi, weight));
        }
        public void DeleteEdge(int vi, int vj)
        {
            _adjacentList[vi].Remove(new AdjacentVertex(vj, 0));
        }
    }
    enum InputFileType
    {
        EdgesList,
        AdjacencyMatrix,
        AdjacencyList
    };
}
