using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_9_2
{
    public class Matrix
    {
        private Dictionary<int,int[]> _matrix;
        private List<int> _columnNumbers;
        public int ColumnsCount { get { return _columnNumbers.Count; } }
        public Edge GetLastEdge()
        {
            return new Edge(_matrix.Keys.ElementAt(0), _columnNumbers[0]);
        }
        Matrix()
        {
            _matrix = new Dictionary<int, int[]>();
            _columnNumbers = new List<int>();
        }
        public Matrix(int[][] matrix)
        {
            this._matrix = new Dictionary<int, int[]>();
            this._columnNumbers = new List<int>();
            for (int i = 0; i < matrix.Length; i++) 
            {
                this._matrix[i] = matrix[i];
                _columnNumbers.Add(i);
            }
        }
        public Matrix(Matrix matrix)
        {
            this._matrix = new Dictionary<int, int[]>(matrix._matrix);
            this._columnNumbers = new List<int>(matrix._columnNumbers);
        }
        public int[] this[int index] => _matrix[index];
        public int GetMinFromRow(int index) => _matrix[index].Min();
        public int GetMinFromRow(int index, int excludedIndex)
        {
            int minValue = int.MaxValue;
            excludedIndex = _columnNumbers.IndexOf(excludedIndex);
            for(int i = 0; i < _matrix[index].Length; i++)
            {
                if (_matrix[index][i] < minValue && i != excludedIndex) minValue = _matrix[index][i];
            }
            return minValue;
        }
        public int GetMinFromColumn(int index) 
        {
            int minValue = int.MaxValue;
            index = _columnNumbers.IndexOf(index);
            foreach(var row in _matrix)
            {
                if (row.Value[index] < minValue) minValue = row.Value[index];
            }
            return minValue;
        }
        public int GetMinFromColumn(int index, int excludedIndex)
        {
            int minValue = int.MaxValue;
            index = _columnNumbers.IndexOf(index);
            foreach (var row in _matrix)
            {
                if (row.Value[index] < minValue && row.Key != excludedIndex) minValue = row.Value[index];
            }
            return minValue;
        }
        public void SumWithRow(int index, int value)
        {
            for (int i = 0; i < _matrix[index].Length; i++)
            {
                if (_matrix[index][i] == int.MaxValue) continue;
                _matrix[index][i] += value;
            }
        }
        public void SumWithColumn(int index, int value)
        {
            index = _columnNumbers.IndexOf(index); 
            foreach (var row in _matrix)
            {
                if (row.Value[index] == int.MaxValue) continue;
                row.Value[index] += value;
            }
        }
        public int RowReduce()
        {
            int sum = 0;
            foreach(var row in _matrix)
            {
                int minValue = GetMinFromRow(row.Key);
                sum += minValue;
                SumWithRow(row.Key,-(minValue));
            }
            return sum;
        }
        public void BanEdge(Edge edge)
        {
            _matrix[edge.vi][_columnNumbers.IndexOf(edge.vj)] = int.MaxValue;
        }
        public int ColumnReduce()
        {
            int sum = 0;
            foreach(int columnNumber in _columnNumbers)
            {
                int minValue = GetMinFromColumn(columnNumber);
                sum += minValue;
                SumWithColumn(columnNumber,-(minValue));
            }
            return sum;
        }
        public Edge[] ZerosMinimum() 
        {
            List<Edge> edges = new List<Edge>();
            foreach (var row in this._matrix)
            {
                int length = row.Value.Length;
                foreach (var i in _columnNumbers)
                {
                    if (row.Value[_columnNumbers.IndexOf(i)] == 0) edges.Add(new Edge(row.Key, i, GetMinFromRow(row.Key,i) + GetMinFromColumn(i,row.Key)));
                }
            }
            return edges.ToArray();
        }
        public Matrix SelectOption(Edge edge)
        {
            Matrix newMatrix = new Matrix();
            newMatrix._columnNumbers = new List<int>(this._columnNumbers);
            newMatrix._columnNumbers.Remove(edge.vj);
            foreach (var row in this._matrix)
            {
                if (row.Key == edge.vi) continue;
                newMatrix._matrix[row.Key] = new int[newMatrix._columnNumbers.Count];
                for (int i = 0; i < row.Value.Length; i++)
                {
                    if (_columnNumbers[i] == edge.vj || i >= row.Value.Length) continue;
                    newMatrix[row.Key][i < _columnNumbers.IndexOf(edge.vj) ? i : i - 1] = this._matrix[row.Key][i];
                }
            }
            int index = newMatrix._columnNumbers.IndexOf(edge.vi);
            if (newMatrix._matrix.ContainsKey(edge.vj) && index != -1)
            newMatrix[edge.vj][index] = int.MaxValue;
            return newMatrix;
        }
        public void PrintMatrix()
        {
            Console.Write("\t"); foreach (var num in this._columnNumbers) Console.Write(num + "\t");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var row in _matrix)
            {
                // Console.Write((char)(row.Key + 65) + "\t");
                Console.Write(row.Key + "\t");
                for (int i = 0; i < row.Value.Length; i++)
                {
                    if (row.Value[i] == int.MaxValue) Console.Write("M\t");
                    else Console.Write(row.Value[i]+"\t");
                }
                Console.WriteLine();
            }
        }
    }
}
