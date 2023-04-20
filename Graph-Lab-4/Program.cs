using GraphLab;
using GraphLab.Components;
using System.Diagnostics.CodeAnalysis;
using System.Text;

Graph graph = new Graph("C:\\Users\\user\\source\\repos\\Graph-Labs\\Graph-Lab-4\\task4\\matrix_t4_001.txt", InputFileType.AdjacencyMatrix);
graph = graph.CorrelatedGraph();
//Console.WriteLine(graph.GetConnectedness());
Edge[] ed = graph.Boruvka();
int sum = 0;
foreach (Edge edge in ed)
{
    Console.WriteLine(edge);
    sum += edge.weight;
}
Console.WriteLine(sum);