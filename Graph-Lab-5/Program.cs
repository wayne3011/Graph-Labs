using GraphLab.Components;
using System.Text;
using GraphLab;
using Graph_Lab3.Components.OutputModels;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Graph_Lab_5;

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
                    Console.WriteLine("{            ЛАБАРАТОРНАЯ РАБОТА № 4             }");
                    Console.WriteLine("{             АВТОР: ЕРМИЛОВ МАКСИМ              }");
                    Console.WriteLine("{              ГРУППА: М3О-225Бк-21              }");
                    Console.WriteLine("{  -e <path> - ввод графа со списка рёбер        }");
                    Console.WriteLine("{  -m <path> - ввод графа с матрицы смежности    }");
                    Console.WriteLine("{  -l <path> - ввод графа со списка смежности    }");
                    Console.WriteLine("{  -o <path> - добавление ключа к любой команде  }");
                    Console.WriteLine("{              выведет резултат в файл           }");
                    Console.WriteLine("{  -n <vertex_number>  начальная вершина         }");
                    Console.WriteLine("{  -d <vertex_number>  конечная вершина          }");
                    Console.WriteLine("{------------------------------------------------}");
                }
                else Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
            }


            MultiWriter multiWriter;
            if (args.Length == 8)
            {
                if (args[6] == "-o")
                {
                    multiWriter = new MultiWriter(new List<Stream>() { new StreamWriter(args[7]).BaseStream, Console.OpenStandardOutput() });
                }
                else
                {
                    Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
                    return;
                }

            }
            else
            {
                multiWriter = new MultiWriter(new List<Stream>() { Console.OpenStandardOutput() });
            }

            if (args[2] != "-n" && args[4] != "-d")
            {
                Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
                return;
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
                    graph = new Graph();
                    break;
            }

            int beginVertex = int.Parse(args[3]);
            int endVertex = int.Parse(args[5]);

            Result result = Dijkstra.Run(graph, beginVertex-1, endVertex-1);
            if (result.Length == int.MaxValue)
            {
                Console.WriteLine($"There is no path between the vertices {beginVertex} and {endVertex}.");
                return;
            }
            Console.WriteLine($"Shortest path length between {beginVertex} and {endVertex} vertices: " + result.Length);
            Console.WriteLine("Path:");
            Console.WriteLine(FormatEdgesArray(result.Path));
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
                sb.AppendFormat("({0}, {1}, {2}), ", array[i].vi + 1, array[i].vj + 1, array[i].weight);
            }
            sb.AppendFormat("({0}, {1}, {2})]", array[array.Length - 1].vi + 1, array[array.Length - 1].vj + 1, array[array.Length - 1].weight);
            return sb.ToString();
        }

    }
}