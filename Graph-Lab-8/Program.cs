using Graph_Lab_8;
using Graph_Lab_8.Components;
using GraphLab;

namespace Graph_Lab_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>(args);
            int indexOfInputPath = list.IndexOf("-m");
            if (indexOfInputPath == -1) Console.WriteLine("Отсутствует обязательный ключ -m!");
            int indexOfBeginCell = list.IndexOf("-n");
            if (indexOfBeginCell == -1) Console.WriteLine("Отсутствует обязательный ключ -n!");
            int indexOfEndCell = list.IndexOf("-d");
            if (indexOfEndCell == -1) Console.WriteLine("Отсуствует обязательный ключ -d!");
            int indexOfOutputPath = list.IndexOf("-o");
            StreamWriter outputStream;
            if(indexOfOutputPath == -1)
            {
                outputStream = new StreamWriter(Console.OpenStandardOutput());
            }
            else
            {
                outputStream = new StreamWriter(args[indexOfOutputPath+1]);
            }
            Map map = new Map(args[indexOfInputPath+1]);
            int beginCell = int.Parse(args[indexOfBeginCell +1]);
            int endCell = int.Parse(args[indexOfEndCell +1]);
            Func<Cell, Cell, int>[] Heuristics = { EuclideanHeuristic, ManhattanHeuristics, ChebyshevaHeuristics, Dij };
            (Cell[] result, int weight, int visitedCells) = AStar.Run(map, beginCell, endCell);
            
        }
        static int EuclideanHeuristic(Cell cell1, Cell cell2) => (int)Math.Sqrt(Math.Pow((cell1.X - cell2.X), 2) + Math.Pow((cell1.Y - cell2.Y), 2));
        static int ManhattanHeuristics(Cell cell1, Cell cell2) => Math.Abs(cell1.X - cell2.X) + Math.Abs(cell1.Y - cell2.Y);
        static int ChebyshevaHeuristics(Cell cell1, Cell cell2) => Math.Max(Math.Abs(cell1.X - cell2.X), Math.Abs(cell1.Y - cell2.Y));
        static int DijkstraHeuristics(Cell cell1, Cell cell2) => 0;
    }

}

