using GraphLab.Components;
using System.Text;
using GraphLab;
namespace GraphLab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("C:\\Users\\user\\source\\repos\\Graph-Labs\\Graph-Lab3\\task3\\matrix_t3_001.txt", InputFileType.AdjacencyMatrix);
            Console.WriteLine(FormatEdgesArray(graph.BridgeSearch()));
        } 
        static string FormatEdgesArray(Edge[] array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < array.Length - 1; i++)
            {
                sb.AppendFormat("({0}, {1}), ", array[i].vi, array[i].vj);
            }
            sb.AppendFormat("({0}, {1})]", array[array.Length-1].vi, array[array.Length-1].vj);
            return sb.ToString();
        }
    }
}