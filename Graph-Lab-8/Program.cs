using Graph_Lab_8;
using Graph_Lab_8.Components;
using GraphLab;
using System.Runtime.CompilerServices;
using System.Text;

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
            int cellsCount = (int)(Math.Pow(map.CellsCount,2));
            Cell beginCell = new Cell(int.Parse(args[indexOfBeginCell +1]), int.Parse(args[indexOfBeginCell + 2]));
            Cell endCell = new Cell(int.Parse(args[indexOfEndCell +1]), int.Parse(args[indexOfEndCell + 2]));
            Func<Cell, Cell, int>[] Heuristics = { EuclideanHeuristic, ManhattanHeuristics, ChebyshevaHeuristics, DijkstraHeuristics };
            foreach(var heuristic in Heuristics)
            {
                Console.WriteLine(heuristic.Method.Name + ":");
                (Cell[] result, int weight, int visitedCells) = AStar.Run(map, beginCell, endCell,heuristic);
                Console.WriteLine($"{weight} - length of path between {beginCell} and {endCell} points.");
                Console.WriteLine("Path:");
                Console.WriteLine(FormatOuputCellsArray(result));
                Console.WriteLine($"Percentage of cells visited: {visitedCells}/{cellsCount} * 100% = " + ((double)visitedCells / (double)cellsCount) * 100 + "%");
                Console.WriteLine();
            }
            
            
        }
        static string FormatOuputCellsArray(Cell[] cells)
        {
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < cells.Length - 1; i++)
            {
                sb.Append(cells[i] + ", ");
            }
            sb.Append(cells[cells.Length-1] + "]");
            return sb.ToString();
        }
        static int EuclideanHeuristic(Cell cell1, Cell cell2) => (int)Math.Sqrt(Math.Pow((cell1.X - cell2.X), 2) + Math.Pow((cell1.Y - cell2.Y), 2));
        static int ManhattanHeuristics(Cell cell1, Cell cell2) => Math.Abs(cell1.X - cell2.X) + Math.Abs(cell1.Y - cell2.Y);
        static int ChebyshevaHeuristics(Cell cell1, Cell cell2) => Math.Max(Math.Abs(cell1.X - cell2.X), Math.Abs(cell1.Y - cell2.Y));
        static int DijkstraHeuristics(Cell cell1, Cell cell2) => 0;
    }

}

