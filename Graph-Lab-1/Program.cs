using System.Text;

namespace GraphLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Graph graph = new Graph("C:\\Users\\Максим\\source\\repos\\Graph-lab-1\\Graph-Lab-1\\Test1\\matrix_t1_008.txt", InputFileType.AdjacencyMatrix);
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
            Console.WriteLine(graph.IsDirected());
            PrintMatrix(graph.GetAdjacencyMatrixForWarshall());
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



    }
}