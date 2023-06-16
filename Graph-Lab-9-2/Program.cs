using Graph_Lab_9_2;
using GraphLab;
using GraphLab.Components;

Graph graph = new Graph("C:\\Users\\user\\source\\repos\\Graph-Labs\\Graph-Lab-9-2\\task9\\matrix_t9_004.txt", InputFileType.AdjacencyMatrix);
BranchesAndBoundaries bb = new BranchesAndBoundaries(graph.GetAdjacencyMatrixForWarshall());
List<Edge> path = bb.Run().ToList();
int sum = 0;
//foreach(var edge in path)
//{

//    Console.WriteLine(edge);
//    sum += graph.Weight(edge.vi,edge.vj);
//}
Console.WriteLine(sum);
List<Edge> processedPath = new List<Edge>();
Edge currentElement = path[0];
while (path.Count != 0)
{
    path.Remove(currentElement);
    processedPath.Add(new Edge(currentElement.vi, currentElement.vj, graph.Weight(currentElement.vi, currentElement.vj)));
    currentElement = path.FirstOrDefault(el => el.vi == currentElement.vj, new Edge(0, 0, 0));
}
foreach (var en in processedPath)
{
    Console.WriteLine(en);
}
