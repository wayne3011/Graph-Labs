using GraphLab.Components;
namespace GraphLab
{
    internal class Graph
    {
        private readonly SortedSet<AdjacentVertex>[] _adjacentList; 
        public bool IsDirected { get; }
        public Graph(string filePath, InputFileType fileType)
        {
            
            if (fileType == InputFileType.AdjacencyMatrix)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string matrix = reader.ReadToEnd() ?? string.Empty;
                    int i = 0;
                    List<SortedSet<AdjacentVertex>> adjacentList = new List<SortedSet<AdjacentVertex>>();
                    foreach(var line in matrix.Split("\n"))
                    {
                        int j = 0;
                        SortedSet<AdjacentVertex> adjacentVerticeSet = new SortedSet<AdjacentVertex>();
                        if (line == string.Empty) continue;
                        foreach (var number in line.Trim('\r',' ').Split(' '))
                        {
                            int weight = Int32.Parse(number);
                            if (weight != 0) adjacentVerticeSet.Add(new AdjacentVertex(j,weight));
                            j++;
                        }
                        adjacentList.Add(adjacentVerticeSet);
                        i++;
                    }
                    _adjacentList = adjacentList.ToArray();
                }
            }

            else if(fileType == InputFileType.EdgesList)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    Dictionary<int,SortedSet<AdjacentVertex>> adjacentList = new Dictionary<int, SortedSet<AdjacentVertex>>();
                    string Edges = reader.ReadToEnd() ?? string.Empty;
                    foreach(var line in Edges.Split('\n'))
                    {
                        if (line == string.Empty) continue;
                        string[] values = line.Trim(' ').Split(' ');
                        if (values.Length != 3) throw new Exception("Invalid input data!");
                        int vi = int.Parse(values[0]) - 1;
                        int vj = int.Parse(values[1]) - 1;
                        int weight = int.Parse(values[2]);
                        if (!adjacentList.ContainsKey(vi))
                        {
                            adjacentList.Add(vi, new SortedSet<AdjacentVertex>());
                        }
                        adjacentList[vi].Add(new AdjacentVertex(vj,weight));
                    }
                    _adjacentList = adjacentList.Values.ToArray();
                }
            }

            else if(fileType == InputFileType.AdjacencyList)
            {
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
                        }
                        i++;
                    }
                    _adjacentList = adjacentList.ToArray();
                }
            }
            IsDirected = CheckDirected();           
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
            return _adjacentList[vi].FirstOrDefault(vertex => vertex.Vj == vj, new AdjacentVertex(0, 0)).Weight;
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
        static public int[] GetEccentricity(int[][] FloydWarshallMatrix)
        {
            int[] eccentricity = new int[FloydWarshallMatrix.Length];
            for (int i = 0; i < FloydWarshallMatrix.Length; i++)
            {
                eccentricity[i] = FloydWarshallMatrix[i].Max();
            }
            return eccentricity;
        }
        
        public int GetRadius()
        {
            return GetEccentricity().Min();
        }        
        public int GetDiameter()
        {
            return GetEccentricity().Max();
        }
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
    }
    enum InputFileType
    {
        EdgesList,
        AdjacencyMatrix,
        AdjacencyList
    };

}
