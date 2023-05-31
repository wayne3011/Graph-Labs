using Graph_Lab_11;
using GraphLab;
using GraphLab.Components;

Graph graph = new Graph("C:\\Users\\Максим\\source\\repos\\Graph-lab-1\\Graph-Lab-11\\task11\\matrix_t11_001.txt",InputFileType.AdjacencyMatrix);
MaximumMatchings maximumMatchings = new MaximumMatchings(CorrelatedGraph(graph));
int[][] matrix = CorrelatedGraph(graph).GetAdjacencyMatrix();
for (int i = 0; i < matrix.Length; i++)
{
    for (int j = 0; j < matrix.Length; j++)
    {
        Console.Write(matrix[i][j] + "\t");
    }
    Console.WriteLine();
}
int[] FirstShare;
int[] SecondShare;
(FirstShare,SecondShare) = maximumMatchings.CheckBipartite();
return 0;
