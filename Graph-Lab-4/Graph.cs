using Graph_Lab3.Components.OutputModels;
using GraphLab.Components;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace GraphLab
{
    internal class Graph
    {
        private readonly SortedSet<AdjacentVertex>[] _adjacentList;
        private int[] _vetexComponents;
        public bool IsDirected { get; }
        private int _edgeCount;
        public int EdgeCount { get { return _edgeCount; } }
        Graph(int vertexCount)
        {
            IsDirected = false;
            _adjacentList = new SortedSet<AdjacentVertex>[vertexCount];
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                _adjacentList[i] = new SortedSet<AdjacentVertex>();
            }
            _vetexComponents = new int[vertexCount];
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
                            _edgeCount++;
                        }
                        _adjacentList = adjacentList.Values.ToArray();
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
                            foreach (var number in line.Trim('\r', ' ').Split(' '))
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
                    using(StreamReader reader = new StreamReader(filePath))
                    {
                        List<SortedSet<AdjacentVertex>> adjacentList = new List<SortedSet<AdjacentVertex>>();
                        string AdjacentVertices = reader.ReadToEnd() ?? string.Empty;
                        int i = 0;
                        foreach(var line in AdjacentVertices.Split('\n'))
                        {
                            if (line == string.Empty) continue;
                            string[] values = line.Trim(' ').Split(' ');
                            adjacentList.Add(new SortedSet<AdjacentVertex>());
                            foreach(var value in values)
                            {
                                adjacentList[i].Add(new AdjacentVertex(int.Parse(value)-1, 1));
                                _edgeCount++;
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
            _vetexComponents = new int[_adjacentList.Length];
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
            _vetexComponents = new int[graph._adjacentList.Length];
        }
        /// <summary>
        /// Return the adjacency matrix of the Graph
        /// </summary>
        /// <returns>Two-dimensional array</returns>
        public int[][] GetAdjacencyMatrix()
        {
            int[][] matrix = new int[_adjacentList.Length][];
            int i = 0;
            foreach(var vertex in _adjacentList)
            {
                matrix[i] = new int[_adjacentList.Length]; 
                foreach(var element in _adjacentList[i])
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
                        AdjacentVertex vertex = _adjacentList[i].FirstOrDefault(v => v.Vj == j,new AdjacentVertex(j,int.MaxValue));
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
            foreach(var adjacentVertex in _adjacentList[vertex])
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
            foreach(var adjacentVertices in _adjacentList)
            {
                foreach(var vertex in adjacentVertices)
                {
                    listOfEdges.Add(new Edge(i, vertex.Vj,vertex.Weight));
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
                foreach(var adjacentVertex in _adjacentList[i])
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
        /// <summary>
        /// Returns the result of the Floyd Warshall algorithm as a distance matrix
        /// </summary>
        /// <returns></returns>
        public int[][] FloydWarshallAlgorithm()
        {
            int[][] MatrixD = GetAdjacencyMatrixForWarshall();
            for (int k = 0; k < _adjacentList.Length; k++)
            {
                for (int i = 0; i < _adjacentList.Length; i++)
                {
                    for (int j = 0; j < _adjacentList.Length; j++)
                    {
                        if (MatrixD[i][k] == int.MaxValue || MatrixD[k][j] == int.MaxValue) continue;
                        MatrixD[i][j] = Math.Min(MatrixD[i][j], MatrixD[i][k] + MatrixD[k][j]);
                    }
                }
            }
            return MatrixD;
        }
        /// <summary>
        /// Returns the vector of degrees of the <see cref="Graph"/>
        /// </summary>
        /// <returns>
        /// For undirected graph incoimng and outgoing vectors are equal
        /// </returns>
        public DegreeVector GetDegreeVector()
        {
            DegreeVector degreeVector = new DegreeVector(IsDirected,_adjacentList.Length);

            for (int i = 0; i < _adjacentList.Length; i++)
            {
                degreeVector.SetOutgoingVectorDegree(i, _adjacentList[i].Count);
            }
            if (IsDirected)
            {
                for (int i = 0; i < _adjacentList.Length; i++)
                {
                    foreach (var outgoingVertex in _adjacentList[i])
                    {
                        degreeVector.GetIncomingVector()[outgoingVertex.Vj]++;
                    }
                }
            }
          
            return degreeVector;
        }
        /// <summary>
        /// Return the eccenrisity vector of the <see cref="Graph"/>
        /// </summary>
        /// <returns></returns>
        /// <remarks >Note: This method uses Floyd Warshall's O(n^3) algorithm. If you already have a result from it, use the static method with the highest performance</remarks>
        public int[] GetEccentricity()//??? Только связный граф?
        {
            int[][] floydWarshallMatrix = FloydWarshallAlgorithm();
            int[] eccentricity = new int[floydWarshallMatrix.Length];
            for (int i = 0; i < floydWarshallMatrix.Length; i++)
            {
                eccentricity[i] = floydWarshallMatrix[i].Max();
            }
            return eccentricity;
        }
        /// <summary>
        /// Return the eccenrisity vector of the <see cref="Graph"/>
        /// </summary>
        /// <param name="FloydWarshallMatrix">The result of the Floyd Warshell algorithm</param>
        /// <returns></returns>
        static public int[] GetEccentricity(int[][] FloydWarshallMatrix)
        {
            int[] eccentricity = new int[FloydWarshallMatrix.Length];
            for (int i = 0; i < FloydWarshallMatrix.Length; i++)
            {
                eccentricity[i] = FloydWarshallMatrix[i].Max();
            }
            return eccentricity;
        }
        /// <summary>
        /// Gets the radius of the <see cref="Graph"/>
        /// </summary>
        /// <returns></returns>
        public int GetRadius()
        {
            return GetEccentricity().Min();
        }
        /// <summary>
        /// Gets the diameter of the <see cref="Graph"/>
        /// </summary>
        /// <returns></returns>
        public int GetDiameter()
        {
            return GetEccentricity().Max();
        }
        /// <summary>
        /// Gets the vector of the central vertices of the <see cref="Graph"/>
        /// </summary>
        /// <returns></returns>
        public int[] GetCentralVertices()
        {
            int[] eccentricity = GetEccentricity();
            int radius = eccentricity.Min();
            List<int> centralVertices = new List<int>();
            for (int i = 0; i < eccentricity.Length; i++)
            {
                if(eccentricity[i] == radius) centralVertices.Add(i);
            }
            return centralVertices.ToArray();
        }
        /// <summary>
        /// Gets the vector of the pheripheral vertices of the <see cref="Graph"/>
        /// </summary>
        /// <returns></returns>
        public int[] GetPeripheralVertices()
        {
            int[] eccentricity = GetEccentricity();
            List<int> peripheralVertices = new List<int>();
            int diameter = eccentricity.Max();
            for (int i = 0; i < eccentricity.Length; i++)
            {
                if (eccentricity[i] == diameter) peripheralVertices.Add(i);
            }
            return peripheralVertices.ToArray();
        }
        public void AddEdge(int vi, int vj, int weight)
        {
            if (!_adjacentList[vi].Contains(new AdjacentVertex(vj, 0))) _edgeCount++;
            _adjacentList[vi].Add(new AdjacentVertex(vj, weight));
            if (!IsDirected) _adjacentList[vj].Add(new AdjacentVertex(vi, weight));
        }
        public void AddEdge(Edge newEdge)
        {
            if (!_adjacentList[newEdge.vi].Contains(new AdjacentVertex(newEdge.vj, 0))) _edgeCount++;
            _adjacentList[newEdge.vi].Add(new AdjacentVertex(newEdge.vj, newEdge.weight));
            if(!IsDirected) _adjacentList[newEdge.vj].Add(new AdjacentVertex(newEdge.vi, newEdge.weight));

        }
        public int[][] GetConnectivityСomponents()
        {
            List<List<int>> components = new List<List<int>>();
            HashSet<int> unvisitedVertices = new HashSet<int>();
            for (int j = 0; j < _adjacentList.Length; j++)
            {
                unvisitedVertices.Add(j);
            }
            Queue<int> queue = new Queue<int>();
            int i = 0;
            while(unvisitedVertices.Count != 0)
            {
                int unvisitedVertex = unvisitedVertices.First();
                queue.Enqueue(unvisitedVertex);
                unvisitedVertices.Remove(unvisitedVertex);
                components.Add(new List<int>() { unvisitedVertex });
                while (queue.Count != 0)
                {
                    int currentVertex = queue.Dequeue();
                    foreach (var neighboringVertex in _adjacentList[currentVertex])
                    {
                        if (unvisitedVertices.Contains(neighboringVertex.Vj))
                        {
                            unvisitedVertices.Remove(neighboringVertex.Vj);
                            queue.Enqueue(neighboringVertex.Vj);
                            components[i].Add(neighboringVertex.Vj);
                            _vetexComponents[neighboringVertex.Vj] = i;
                        }
                    }
                }
                i++;
            }
            int[][] result = new int[components.Count][];
            for (int k = 0; k < components.Count; k++)
            {
                result[k] =  components[k].ToArray();
            }
            return result;
            
        }
        public bool GetConnectedness()
        {
            return GetConnectivityСomponents().Length == 1;
        }
        public Graph CorrelatedGraph()
        {
            Graph correlatedGraph = new Graph(this);
            if (!IsDirected) return correlatedGraph;
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                foreach(var adjacentVertex in _adjacentList[i])
                {
                    correlatedGraph._adjacentList[adjacentVertex.Vj].Add(new AdjacentVertex(i, adjacentVertex.Weight));
                }
                
            }
            return correlatedGraph;
        }
        public Graph Inverse()
        {
            Graph inverseGraph = new Graph(this._adjacentList.Length);
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                foreach(var adjacentVertex in _adjacentList[i])
                {
                    inverseGraph._adjacentList[adjacentVertex.Vj].Add(new AdjacentVertex(i, adjacentVertex.Weight));
                }
            }
            return inverseGraph;
        }
        private int ClockDfs(int currentVertex, int[] vertices, ref int time)
        {
            vertices[currentVertex] = time;
            foreach (var adjacentVertex in _adjacentList[currentVertex])
            {
                if (vertices[adjacentVertex.Vj] == 0) ClockDfs(adjacentVertex.Vj, vertices, ref time);
            }
            vertices[currentVertex] = time;
            time++;
            return time;
        }
        private void DfsKosarayu(List<int> component, int vertex, int[] vertices)
        {
            vertices[vertex] = int.MinValue;
            foreach(var adjacentVertex in _adjacentList[vertex])
            {
                if (vertices[adjacentVertex.Vj]!=int.MinValue) DfsKosarayu(component, adjacentVertex.Vj, vertices);
            }
            component.Add(vertex);  
        }
        public int[][] Kosarayu()
        {
            Graph graph = this.Inverse();
            int[] vertices = new int[this._adjacentList.Length];
            int time = 1;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i] == 0) graph.ClockDfs(i, vertices, ref time);
            }
            

            List<List<int>> components = new List<List<int>>();
            int k = 0;
            int newVertex = -1;
            while ((newVertex = vertices.FirstMax()) >= 0)
            {
                components.Add(new List<int>());
                DfsKosarayu(components[k], newVertex, vertices);
                k++;
            }
            int[][] result = new int[components.Count][];
            for (int i = 0; i < components.Count; i++)
            {
                result[i] = components[i].ToArray();
            }
            return result;
            PriorityQueue<int, int> pq = new PriorityQueue<int, int>();
        }
        private void BridgesAndHingesDfs(int currentVertex, int parentVertex, int[] tin, int[] tup, ref int time, bool[] used, List<Edge> briges, HashSet<int> hinges, ref int firstVertexCounter, ref int firstVertex)
        {
            tup[currentVertex] = tin[currentVertex] = time;
            time++;
            used[currentVertex] = true;
            foreach(AdjacentVertex adjacentVertex in _adjacentList[currentVertex])
            {
                if (adjacentVertex.Vj == parentVertex) continue;
                else if (used[adjacentVertex.Vj]) tup[currentVertex] = Math.Min(tup[currentVertex], tin[adjacentVertex.Vj]);
                else
                {
                    BridgesAndHingesDfs(adjacentVertex.Vj,currentVertex,tin,tup,ref time,used,briges,hinges,ref firstVertexCounter, ref firstVertex);
                    tup[currentVertex] = Math.Min(tup[currentVertex], tup[adjacentVertex.Vj]);
                    if(tup[adjacentVertex.Vj] > tin[currentVertex]) briges.Add(new Edge(currentVertex,adjacentVertex.Vj));
                    if (tup[adjacentVertex.Vj] >= tin[currentVertex])
                    {
                        if(currentVertex != firstVertex || firstVertexCounter == 1) hinges.Add(currentVertex);
                        else firstVertexCounter++;
                    }
                }
            }
        }
        public BridgesAndHingesResult BridgeAndHingesSearch()
        {
            bool[] used = new bool[_adjacentList.Length];
            int[] tin = new int[_adjacentList.Length];
            int[] tup = new int[_adjacentList.Length];
            List<Edge> bridges = new List<Edge>();
            HashSet<int> hinges = new HashSet<int>();
            BridgesAndHingesResult result = new BridgesAndHingesResult();
            int time = 1;

            for (int i = 0; i < _adjacentList.Length; i++)
            {
                int firstVertexCounter = 0;
                if (used[i] == false) BridgesAndHingesDfs(i, -1, tin, tup, ref time, used, bridges, hinges, ref firstVertexCounter, ref i);
            }
            result.Bridges = bridges.ToArray();
            result.Hinges = hinges.ToArray();
            return result;
        }
        public Edge[] Kruscala()
        {
            Queue<Edge> edges = new Queue<Edge>(GetListOfEdges().OrderBy(c => c.weight));
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
        public Edge[] Prima()
        {
            List<Edge> tree = new List<Edge>();
            HashSet<int> visited = new HashSet<int>();
            bool[] used = new bool[_adjacentList.Length]; 
            visited.Add(0);
            while(tree.Count != _adjacentList.Length - 1)
            {
                int minWeightEdge = int.MaxValue;
                int minWeightVertexNumber = -1;
                int minWeigthVertexParent = -1;
                foreach(var vertex in visited)
                { 
                    foreach(var adjacentVertex in _adjacentList[vertex])
                    {
                        if (!visited.Contains(adjacentVertex.Vj) && Weight(vertex,adjacentVertex.Vj) < minWeightEdge)
                        {
                            minWeightEdge = Weight(vertex, adjacentVertex.Vj);
                            minWeightVertexNumber = adjacentVertex.Vj;
                            minWeigthVertexParent = vertex;
                        }
                    }
                }
                used[minWeightVertexNumber] = true;
                tree.Add(new Edge(minWeigthVertexParent, minWeightVertexNumber, Weight(minWeigthVertexParent, minWeightVertexNumber)));
                visited.Add(minWeightVertexNumber);
                
            }
            return tree.ToArray();
        }
        public int[] GetBoruvkaComponents(out int componentsCount)
        {
            //List<List<int>> components = new List<List<int>>();
            int[] components = new int[_adjacentList.Length];
            bool[] visited = new bool[_adjacentList.Length];
            int j = 0;
            for (int i = 0; i < _adjacentList.Length; i++)
            {
                if (!visited[i])
                {
                    GetBoruvkaComponentsDFS(j,visited,i,components);
                    j++;
                }
            }
            componentsCount = j;
            return components;
        }
        private void GetBoruvkaComponentsDFS(int currentComponent, bool[] visited, int currentVertex, int[] components)
        {
            visited[currentVertex] = true;
            components[currentVertex] = currentComponent;
            foreach(var adjacentVertex in _adjacentList[currentVertex])
            {
                if(!visited[adjacentVertex.Vj]) GetBoruvkaComponentsDFS(currentComponent, visited, adjacentVertex.Vj,components);
            }
        }
        public Edge[] Boruvka()
        {
            //List<Edge> tree = new List<Edge>();
            Graph tree = new Graph(_adjacentList.Length);
            //List<List<int>> components = new List<List<int>>();
            int[] components;
            Edge[] edges = GetListOfEdges();
            while (tree.EdgeCount != _adjacentList.Length - 1)
            {
                int minEdgeSize;
                components = tree.GetBoruvkaComponents(out minEdgeSize);
                Edge[] minEdge = new Edge[minEdgeSize];//?? MAP
                for(int i = 0; i < minEdge.Length; i++)
                {
                    minEdge[i] = new Edge(0,0,int.MaxValue);
                }
                for (int i = 0; i < edges.Length; i++)
                {
                    if (components[edges[i].vi] != components[edges[i].vj])
                    {
                        if (minEdge[components[edges[i].vi]].weight > edges[i].weight)
                        {
                            minEdge[components[edges[i].vi]] = edges[i];
                        }
                        if (minEdge[components[edges[i].vj]].weight > edges[i].weight)
                        {
                            minEdge[components[edges[i].vj]] = edges[i];
                        }
                    }
                }
                for(int i = 0; i < minEdge.Length; i++)
                {
                    if (minEdge[i].weight != int.MaxValue) tree.AddEdge(minEdge[i]);
                    if (minEdge[i].weight != int.MaxValue) tree.AddEdge(minEdge[i]);
                }
                
            }
            return tree.GetListOfEdges();
        }

    }
    enum InputFileType
    {
        EdgesList,
        AdjacencyMatrix,
        AdjacencyList
    };
    internal static class Extexsions
    {
        public static int FirstMax(this int[] array)
        {
            int indexOfMax = -1;
            int maxValue = int.MinValue;
            for (int i = 0; i < array.Length; i++)
            {
                if(array[i] > maxValue)
                {
                    maxValue = array[i];
                    indexOfMax = i;
                }
            }
            return indexOfMax;
            
        }
    }


}
