using GraphLab;
using GraphLab.Components;

Graph graph = new Graph("C:\\Users\\Максим\\source\\repos\\Graph-lab-1\\Graph-Lab-4\\task4\\matrix_t4_001.txt", InputFileType.AdjacencyMatrix);
Edge[] edfes = graph.Kruscala();
for (int i = 0; i < edfes.Length; i++)
{
    Console.WriteLine(edfes[i]);
}