namespace GraphLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("C:\\Users\\user\\source\\repos\\Graph-lab-1\\Graph-Lab-1\\Test1\\list_of_adjacency_t1_013.txt", InputFileType.AdjacencyList);
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
        }
        static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int  j = 0;  j < matrix[i].Length;  j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
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