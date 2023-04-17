using GraphLab;
using GraphLab.Components;
using System.Text;

namespace GraphLab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (args.Length == 1)
            {
                if (args[0] == "-h")
                {
                    Console.WriteLine("{------------------------------------------------}");
                    Console.WriteLine("{                  ТЕОРИЯ ГРАФОВ                 }");
                    Console.WriteLine("{            ЛАБАРАТОРНАЯ РАБОТА № 2             }");
                    Console.WriteLine("{             АВТОР: ЕРМИЛОВ МАКСИМ              }");
                    Console.WriteLine("{              ГРУППА: М3О-225Бк-21              }");
                    Console.WriteLine("{  -e <path> - ввод графа со списка рёбер        }");
                    Console.WriteLine("{  -m <path> - ввод графа с матрицы смежности    }");
                    Console.WriteLine("{  -l <path> - ввод графа со списка смежности    }");
                    Console.WriteLine("{  -o <path> - добавление ключа к любоый команде }");
                    Console.WriteLine("{              выведет резултат в файл           }");
                    Console.WriteLine("{------------------------------------------------}");
                }
            }
            bool FileOutput = false;
            StringBuilder? stringBuilder = null;
            StreamWriter? stream = null;
            if (args.Length == 4)
            {
                if (args[2] == "-o")
                {
                    stream = new StreamWriter(args[3]);
                    stream.AutoFlush = true;
                    FileOutput = true;
                }
                else
                    Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
            }
            Graph? graph = null;
            switch (args[0])
            {
                case "-e":
                    graph = new Graph(args[1], InputFileType.EdgesList);
                    break;
                case "-l":
                    graph = new Graph(args[1], InputFileType.AdjacencyList);
                    break;
                case "-m":
                    graph = new Graph(args[1], InputFileType.AdjacencyMatrix);
                    break;
                default:
                    Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
                    break;
            }
            int[][] components; string ouputArrays;
            if (graph.IsDirected == false)
            {
                components = graph.GetConnectivityСomponents();
                if (components.Length == 1){
                    Console.WriteLine("Graph is connected.");
                    if(FileOutput) stream.WriteLine("Graph is connected.");
                }
                else
                {
                    Console.WriteLine("Graph is not connected and contains {0} connected components.", components.Length);
                    if(FileOutput) stream.WriteLine("Graph is not connected and contains {0} connected components.", components.Length);
                }
                Console.WriteLine("Connected components:");
                if(FileOutput) stream.WriteLine("Connected components:");
                ouputArrays = FormatTwoDimensionalArray(components);
                Console.WriteLine(ouputArrays);
                if (FileOutput) stream.WriteLine(ouputArrays);
                return;
            }

            Graph correlatedGraph = graph.CorrelatedGraph();
            components = correlatedGraph.GetConnectivityСomponents();
            if (components.Length == 1)
            {
                Console.WriteLine("Digraph is connected.");
                if (FileOutput) stream.WriteLine("Digraph is connected.");
            }
            else
            {
                Console.WriteLine("Digraph is not connected and contains {0} connected components.", components.Length);
                if (FileOutput) stream.WriteLine("Digraph is not connected and contains {0} connected components.", components.Length);
            }
            Console.WriteLine("Connected components:");
            if (FileOutput) stream.WriteLine("Connected components:");
            ouputArrays = FormatTwoDimensionalArray(components);
            Console.WriteLine(ouputArrays);
            if (FileOutput) stream.WriteLine(ouputArrays);

            components = graph.Kosarayu();
            if (components.Length == 1)
            {
                Console.WriteLine("Digraph is stronly connected.");
                if (FileOutput) stream.WriteLine("Digraph is stronly connected.");
            }
            else
            {
                Console.WriteLine("Digraph is weakly connected and contains {0} strongly connected components.", components.Length);
                if (FileOutput) stream.WriteLine("Digraph is weakly connected and contains {0} strongly connected components.", components.Length);
            }
            Console.WriteLine("Strongly connected components:");
            if (FileOutput) stream.WriteLine("Strongly connected components:");
            ouputArrays = FormatTwoDimensionalArray(components);
            Console.WriteLine(ouputArrays);
            if (FileOutput) stream.WriteLine(ouputArrays);
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
                result.AppendFormat("{0}, ", array[i]+1);
            }
            result.AppendFormat("{0}]", array[array.Length - 1]+1);
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
