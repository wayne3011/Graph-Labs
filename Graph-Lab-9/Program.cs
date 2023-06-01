using Graph_Lab_9;
using GraphLab;

Graph graph = new Graph("C:\\Users\\user\\source\\repos\\Graph-Labs\\Graph-Lab-9\\task9\\matrix_t9_001.txt", InputFileType.AdjacencyMatrix);
Console.WriteLine("GAY!");
AntSystem antSystem = new AntSystem(graph, 2);
foreach(var edge in antSystem.Run())
{
    Console.WriteLine(edge);
}