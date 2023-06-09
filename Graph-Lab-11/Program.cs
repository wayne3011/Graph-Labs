﻿using Graph_Lab_10;
using Graph_Lab_11;
using GraphLab;
using GraphLab.Components;
using System.Text;

namespace GraphLab
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
                    Console.WriteLine("{            ЛАБАРАТОРНАЯ РАБОТА № 10            }");
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
                    graph = new Graph();
                    Console.WriteLine("Неверная комбинация ключей. Используйте -h для получения справки");
                    break;
            }
            try
            {
                MaximumMatchings mm = new MaximumMatchings(graph);
                Edge[] matchings = mm.Run();
                Console.WriteLine("Graph is bipartite");
                Console.WriteLine("Matching count: " + matchings.Length);
                foreach (var edge in matchings)
                {
                    Console.WriteLine(edge.vi + " - " + edge.vj);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Graph is not bipartite!");
                return;
            }


        }

    }

}
