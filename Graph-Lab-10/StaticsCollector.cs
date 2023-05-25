using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLab
{
    internal class StaticsCollector
    {
        private readonly Graph graph;
        public StaticsCollector(Graph graph)
        {
            this.graph = graph;
        }
        /// <summary>
        /// Returns the result of the Floyd Warshall algorithm as a distance matrix
        /// </summary>
        /// <returns></returns>
        public int[][] FloydWarshallAlgorithm()
        {
            int[][] MatrixD = GetAdjacencyMatrixForWarshall();
            for (int k = 0; k < graph.AdjacentList.Length; k++)
            {
                for (int i = 0; i < graph.AdjacentList.Length; i++)
                {
                    for (int j = 0; j < graph.AdjacentList.Length; j++)
                    {
                        if (MatrixD[i][k] == int.MaxValue || MatrixD[k][j] == int.MaxValue) continue;
                        MatrixD[i][j] = Math.Min(MatrixD[i][j], MatrixD[i][k] + MatrixD[k][j]);
                    }
                }
            }
            return MatrixD;
        }
        /// <summary>
        /// Return the adjacency matrix of the Graph
        /// </summary>
        /// <returns>Return the adjacency matrix, where is there no path, int.MaxValue is used</returns>
        private int[][] GetAdjacencyMatrixForWarshall()
        {
            int[][] matrix = new int[graph.AdjacentList.Length][];
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                matrix[i] = new int[graph.AdjacentList.Length];
                for (int j = 0; j < graph.AdjacentList.Length; j++)
                {
                    if (i == j) matrix[i][j] = 0;
                    else
                    {
                        AdjacentVertex vertex = graph.AdjacentList[i].FirstOrDefault(v => v.Vj == j, new AdjacentVertex(j, int.MaxValue));
                        matrix[i][j] = vertex.Weight;
                    }

                }
            }
            return matrix;
        }
        /// <summary>
        /// Returns the vector of degrees of the <see cref="Graph"/>
        /// </summary>
        /// <returns>
        /// For undirected graph incoimng and outgoing vectors are equal
        /// </returns>
        public DegreeVector GetDegreeVector()
        {
            DegreeVector degreeVector = new DegreeVector(graph.IsDirected, graph.AdjacentList.Length);

            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                degreeVector.SetOutgoingVectorDegree(i, graph.AdjacentList[i].Count);
            }
            if (graph.IsDirected)
            {
                for (int i = 0; i < graph.AdjacentList.Length; i++)
                {
                    foreach (var outgoingVertex in graph.AdjacentList[i])
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
                if (eccentricity[i] == radius) centralVertices.Add(i);
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
    }
}
