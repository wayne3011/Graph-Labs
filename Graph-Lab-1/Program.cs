using System.Text;

namespace GraphLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Graph graph = new Graph("C:\\Users\\user\\source\\repos\\Graph-lab-1\\Graph-Lab-1\\Test1\\matrix_t1_001.txt", InputFileType.AdjacencyMatrix);
            int[][] matrix = graph.GetAdjacencyMatrix();
            PrintMatrix(matrix);
            Console.WriteLine(graph.Weight(0, 0));
            Console.WriteLine(graph.Weight(0, 1));
            Console.WriteLine(graph.IsEdge(1, 0));
            Console.WriteLine(graph.IsEdge(4, 3));
            // PrintArray(graph.GetAdjacencyList(0));
            //PrintArray(graph.GetAdjacencyList(4));
            //PrintArray(graph.GetListOfEdges());
            PrintArray(graph.GetListOfEdges(3));
            Console.WriteLine("Is Directed: " + graph.IsDirected);
            Console.WriteLine("Floyd-Warshall:");
            PrintMatrix(graph.FloydWarshallAlgorithm());
            Console.WriteLine("Incoming Degree Vector: ");
            PrintArray(graph.GetDegreeVector().GetIncomingVector());
            Console.WriteLine("Outgoing Degree Vector: ");
            PrintArray(graph.GetDegreeVector().GetOutgoingVector());
            Console.WriteLine("Eccentisity: ");
            PrintArray(graph.GetEccentricity());
            Console.WriteLine("Radius = " + graph.GetRadius());
            Console.WriteLine("Diameter = " + graph.GetDiameter());
            Console.Write("Central Vertices: ");
            PrintArray(graph.GetCentralVertices());
            Console.WriteLine("Peripheral Vertices: ");
            PrintArray(graph.GetPeripheralVertices());
            Console.Read();
        }
        static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int  j = 0;  j < matrix[i].Length;  j++)
                {
                    if (matrix[i][j] == int.MaxValue) Console.Write("∞ ",Encoding.UTF8);
                    else Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void PrintArray(object[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }
        static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }



    }
}