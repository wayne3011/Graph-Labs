using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_8.Components
{
    internal struct Cell
    {
        public Cell(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public override string? ToString()
        {
            return $"({X}, {Y})";
        }
        static public bool operator==(Cell cell1, Cell cell2)
        {
            return cell1.X == cell2.X && cell1.Y == cell2.Y;
        }
        static public bool operator!=(Cell cell1, Cell cell2)
        {
            return !(cell1.X == cell2.X && cell1.Y == cell2.Y);
        }

    }
}
