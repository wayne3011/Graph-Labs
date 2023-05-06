// See https://aka.ms/new-console-template for more information
using Graph_Lab_8;
using Graph_Lab_8.Components;

Map map = new Map("C:\\Users\\user\\source\\repos\\Graph-Labs\\Graph-Lab-8\\task8\\map_001.txt");
string answerPath = "C:\\Users\\user\\source\\repos\\Graph-Labs\\Graph-Lab-8\\task8\\ans_001_map_001.txt";
Cell beginCell = new Cell(9, 0);
Cell endCell = new Cell(3, 9);
(Cell[] path, int weight) = AStar.Run(map, beginCell, endCell, DijkstraHeuristics);
Console.WriteLine(path.Length);
string answerString = File.ReadAllText(answerPath);
int answWeight = 0;
foreach (var cellStr in answerString.Split("\n")[2].Split("), "))
{
   
    int x = int.Parse(cellStr.Trim(')', '(', '[', ']').Split(", ")[0]);
    int y = int.Parse(cellStr.Trim(')', '(', '[', ']').Split(", ")[1]);
    answWeight += map[x, y];
}
Console.WriteLine(answWeight + " " + weight);
foreach(Cell cell in path)
{
    Console.WriteLine(cell.ToString());
}
static int EuclideanHeuristic(Cell cell1, Cell cell2)
{
    return (int)Math.Sqrt(Math.Pow((cell1.X - cell2.X), 2) + Math.Pow((cell1.Y - cell2.Y), 2));
}
static int ManhattanHeuristics(Cell cell1, Cell cell2) => Math.Abs(cell1.X - cell2.X) + Math.Abs(cell1.Y - cell2.Y);
static int ChebyshevaHeuristics(Cell cell1, Cell cell2) => Math.Max(Math.Abs(cell1.X - cell2.X), Math.Abs(cell1.Y - cell2.Y));
static int DijkstraHeuristics(Cell cell1, Cell cell2) => 0;
