using GraphLab.Components;
using System.Text;
using GraphLab;
using Graph_Lab3.Components.OutputModels;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Graph_Lab_5;
using Graph_Lab_7;

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
            Edge[] paths;
            bool haveNegativeEdges;
            try
            {
                paths = Johnson.Run(graph, out haveNegativeEdges);
            }
            catch (NegativeCycleException ex)
            {
                multiWriter.WriteLine(ex.Message);
                return;
            }
            
            if (haveNegativeEdges) { multiWriter.WriteLine("Graph contains edges with negative weight."); }
            else multiWriter.WriteLine("Graph does not contain edges with negative weight.");
            multiWriter.WriteLine("Shortest paths lengths:");
            for (int i = 0; i < paths.Length; i++)
            {
                multiWriter.WriteLine((paths[i].vi+1) + " - " + (paths[i].vj+1) + ": " + paths[i].weight);
            }

        }

    }

}