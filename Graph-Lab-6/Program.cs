using Graph_Lab_5;
using Graph_Lab_6;
using GraphLab;
Graph graph = new Graph("C:/Users/user/source/repos/Graph-Labs/Graph-Lab-6/task6/matrix_t6_001.txt",InputFileType.AdjacencyMatrix);
int beginVertex = 5;
int[] markers = BellmanFord.Run(graph, beginVertex);
for (int i = 0; i < markers.Length; i++)
{
    Console.WriteLine(markers[i]);
}