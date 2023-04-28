using GraphLab;
using GraphLab.Components;
using System.Diagnostics.CodeAnalysis;
using System.Text;

Graph graph = new Graph("C:\\Users\\user\\Desktop\\graph.txt", InputFileType.AdjacencyMatrix);
Console.WriteLine(graph.GetListOfEdges().Length);
//graph = graph.CorrelatedGraph();
int[][] components = graph.Kosarayu();
Console.WriteLine(FormatTwoDimensionalArray(components));
//graph = graph.CorrelatedGraph();
////Console.WriteLine(graph.GetConnectedness());
//Edge[] ed = graph.Boruvka();
//int sum = 0;
//foreach (Edge edge in ed)
//{
//    Console.WriteLine(edge);
//    sum += edge.weight;
//}
//Console.WriteLine(sum);
static string FormatTwoDimensionalArray(int[][] array)
{
    StringBuilder sb = new StringBuilder();
    sb.Append('[');
    for (int i = 0; i < array.Length - 1; i++)
    {
        sb.AppendFormat("{0}, ", FormatOutputArray(array[i]));
    }
    sb.Append(FormatOutputArray(array[array.Length - 1]));
    sb.Append(']');
    return sb.ToString();
}
static string FormatOutputArray(int[] array)
{
    StringBuilder result = new StringBuilder("[", array.Length * 2);
    for (int i = 0; i < array.Length - 1; i++)
    {
        result.AppendFormat("{0}, ", array[i] + 1);
    }
    result.AppendFormat("{0}]", array[array.Length - 1] + 1);
    return result.ToString();
}