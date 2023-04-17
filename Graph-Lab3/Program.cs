using GraphLab.Components;
using System.Text;
using GraphLab;
using Graph_Lab3.Components.OutputModels;

namespace GraphLab3
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
            BridgesAndHingesResult result = graph.BridgeAndHingesSearch();
            Console.WriteLine("Bridges:");
            Console.WriteLine(FormatEdgesArray(result.Bridges));
            Console.WriteLine("Cut vertices");
            Console.WriteLine(FormatOutputArray(result.Hinges));
            if (FileOutput)
            {
                stream.WriteLine("Bridges:");
                stream.WriteLine(FormatEdgesArray(result.Bridges));
                stream.WriteLine("Cut vertices");
                stream.WriteLine(FormatOutputArray(result.Hinges));
            }

        } 
        static string FormatEdgesArray(Edge[] array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            if (array.Length == 0)
            {
                sb.Append("]");
                return sb.ToString();
            }
            for (int i = 0; i < array.Length - 1; i++)
            {
                sb.AppendFormat("({0}, {1}), ", array[i].vi+1, array[i].vj+1);
            }
            sb.AppendFormat("({0}, {1})]", array[array.Length-1].vi+1, array[array.Length-1].vj+1);
            return sb.ToString();
        }
        static string FormatOutputArray(int[] array)
        {
            StringBuilder result = new StringBuilder("[", array.Length * 2);
            if (array.Length == 0)
            {
                result.Append("]");
                return result.ToString();
            }
            array = array.OrderBy(c => c).ToArray();
            for (int i = 0; i < array.Length - 1; i++)
            {
                result.AppendFormat("{0}, ", array[i] + 1);
            }
            result.AppendFormat("{0}]", array[array.Length - 1] + 1);
            return result.ToString();
        }
    }
}