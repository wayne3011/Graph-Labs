using GraphLab;
using GraphLab.Components;
using System.Text;

namespace GraphLab2
{
    internal class Program
    {
        static void Main()
        {
            
        }
        static string FormatTwoDimensionalArray(int[][] array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < array.Length-1; i++)
            {
                sb.AppendFormat("{0}, ",FormatOutputArray(array[i]));
            }
            sb.Append(FormatOutputArray(array[array.Length-1]));
            sb.Append(']');
            return sb.ToString();
        }
        static string FormatOutputArray(int[] array)
        {
            StringBuilder result = new StringBuilder("[", array.Length * 2);
            for (int i = 0; i < array.Length - 1; i++)
            {
                result.AppendFormat("{0}, ", array[i]);
            }
            result.AppendFormat("{0}]", array[array.Length - 1]);
            return result.ToString();
        }
        static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == int.MaxValue) Console.Write("∞ ", Encoding.UTF8);
                    else Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
