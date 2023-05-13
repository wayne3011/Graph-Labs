using Graph_Lab_8.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_8
{
    internal class Map
    {
        readonly private int[][] cells;
        public int CellsCount { get { return cells.Length; } }
        public Map(string path)
        {
            string inputFile = File.ReadAllText(path);
            List<List<int>> cells = new List<List<int>>();
            foreach (var str in inputFile.Split("\n"))
            {
                if (str == string.Empty) continue;
                var temp = new List<int>();
                foreach (var number in str.Split(" "))
                {
                    temp.Add(int.Parse(number));
                }
                cells.Add(temp);
            }
            this.cells = new int[cells.Count][];
            for (int i = 0; i < cells.Count; i++)
            {
                this.cells[i] = cells[i].ToArray();
            }
        }
        public Cell[] Neighbors(Cell cell)
        {
            List<Cell> neighnoirs = new List<Cell>();
            if (cell.X - 1 >= 0) neighnoirs.Add(new Cell(cell.X - 1, cell.Y));
            if (cell.Y - 1 >= 0) neighnoirs.Add(new Cell(cell.X, cell.Y - 1));
            if (cell.X + 1 != cells.Length) neighnoirs.Add(new Cell(cell.X + 1, cell.Y));
            if (cell.Y + 1 != cells.Length) neighnoirs.Add(new Cell(cell.X, cell.Y + 1));
            return neighnoirs.ToArray();
        }
        public int this[int i,int j] => cells[i][j];
        public int this[Cell cell] => cells[cell.X][cell.Y];

    }
}
