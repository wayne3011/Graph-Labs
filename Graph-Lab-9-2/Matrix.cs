using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_9_2
{
    internal class Matrix
    {
        private Dictionary<int,int[]> _matrix;
        private HashSet<int> _columnNumbers;
        public int ColumnsCount { get { return _columnNumbers.Count; } }
        Matrix()
        {
            _matrix = new Dictionary<int, int[]>();
            _columnNumbers = new HashSet<int>();
        }
        public Matrix(int[][] matrix)
        {
            this._matrix = new Dictionary<int, int[]>();
            this._columnNumbers = new HashSet<int>();
            for (int i = 0; i < matrix.Length; i++) 
            {
                this._matrix[i] = matrix[i];
                _columnNumbers.Add(i);
            }
        }
        public int[] this[int index] => _matrix[index];
        public int GetMinFromRow(int index) => _matrix[index].Min();
        public int GetMinFromColumn(int index) 
        {
            int minValue = int.MaxValue;
            foreach(var row in _matrix)
            {
                if (row.Value[index] < minValue) minValue = row.Value[index];
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
            foreach (var row in _matrix)
            {
                if (row.Value[index] == int.MaxValue) continue;
                row.Value[index] += value;
            }
        }
        public int RowReduce()
        {
            int sum = 0;
            for (int i = 0; i < _matrix.Count; i++)
            {
                int minValue = GetMinFromRow(i);
                sum += minValue;
                SumWithRow(i,-(minValue));
            }
            return sum;
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
            foreach (var row in _matrix)
            {
                for (int i = 0; i < row.Value.Length; i++)
                {
                    if (row.Value[i] == 0) edges.Add(new Edge(row.Key, i,GetMinFromRow(row.Key) + GetMinFromColumn(i)));
                }
            }
            return edges.ToArray();
        }
        public Matrix SelectOption(Edge edge)
        {
            Matrix newMatrix = new Matrix();
            newMatrix._columnNumbers = this._columnNumbers;
            newMatrix._columnNumbers.Remove(edge.vj);
            foreach (var row in this._matrix)
            {
                if (row.Key == edge.vi) continue;
                newMatrix._matrix[row.Key] = new int[newMatrix._columnNumbers.Count];
                for (int i = 0; i < row.Value.Length; i++)
                {
                    if (i == edge.vj) continue;
                    if (row.Key == edge.vj && i == edge.vi) newMatrix[row.Key][i] = int.MaxValue; 
                    newMatrix[row.Key][i < edge.vj ? i : i - 1] = this._matrix[row.Key][i];
                }
            }
            return newMatrix;
        }
    }
}
