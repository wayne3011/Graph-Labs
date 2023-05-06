using GraphLab.Components;
using System.Text;
using GraphLab;
using Graph_Lab3.Components.OutputModels;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Graph_Lab_5;
using Graph_Lab_6;

namespace GraphLab6
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
                    Console.WriteLine("{  -o <path> - добавление ключа к любоый команде }");
                    Console.WriteLine("{              выведет резултат в файл           }");
                    Console.WriteLine("{  -d          алгоритм Дейкстры                 }");
                    Console.WriteLine("{  -b          алгоритм Беллмана-Форда-Мура      }");
                    Console.WriteLine("{  -l          алгоритм Левита                   }");
                    Console.WriteLine("{  -n          задать начальную вершину          }");
                    Console.WriteLine("{------------------------------------------------}");
                }
            }


            MultiWriter multiWriter;
            if (args.Length == 4)
            {
                if (args[2] == "-o")
                {
                    multiWriter = new MultiWriter(new List<Stream>() { new StreamWriter(args[3]).BaseStream, Console.OpenStandardOutput() });
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
            int actionKeyIndex = args.Length == 7 ? 4 : 2;
            int beginVertexKeyIndex = args.Length == 7 ? 5 : 3;
            int[] paths = new int[0];
            int beginVertex = 0;
            if (args[beginVertexKeyIndex] == "-n")
            {
                beginVertex = int.Parse(args[beginVertexKeyIndex + 1]) - 1;
            }
            else
            {
                Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
                return;
            }
            bool haveNegativeEdges = false;
            for (int vertex = 0; vertex < graph.AdjacentList.Length; vertex++)
            {
                foreach (var edge in graph.AdjacentList[vertex])
                {
                    if (edge.Weight < 0)
                    {
                        haveNegativeEdges = true;
                        break;
                    }
                }
                if (haveNegativeEdges) break;
            }

            try
            {
                switch (args[actionKeyIndex])
                {
                    case "-d":
                        paths = Dijkstra.Run(graph, beginVertex);
                        break;
                    case "-b":
                        paths = BellmanFord.Run(graph, beginVertex);
                        break;
                    case "-l":
                        paths = Dijkstra.Run(graph, beginVertex);
                        paths = Levit.Run(graph, beginVertex);
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Graph contains a negative cycle.");
                return;
                throw;
            }

            if (haveNegativeEdges) multiWriter.WriteLine("Graph contains edges with negative weight.");
            else multiWriter.WriteLine("Graph does not contain edges with negative weight.");

            multiWriter.WriteLine("Shortest paths lengths:");
            for (int i = 0; i < paths.Length; i++)
            {
                if (beginVertex == i) continue;
                Console.WriteLine($"{beginVertex + 1} - {i+1}: " + ((paths[i] == int.MaxValue) ? "∞" : paths[i]));
            }
        }



    }
}