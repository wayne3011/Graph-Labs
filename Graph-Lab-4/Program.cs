using GraphLab.Components;
using System.Text;
using GraphLab;
using Graph_Lab3.Components.OutputModels;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Graph_Lab_4;

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
                    Console.WriteLine("{  -k          алгоритм Крускала                 }");
                    Console.WriteLine("{  -p          алгоритм Прима                    }");
                    Console.WriteLine("{  -b          алгоритм Борувки                  }");
                    Console.WriteLine("{  -s          замер скорости работы             }");
                    Console.WriteLine("{                     всех алгоритмов            }");
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
            if (graph.IsDirected) graph = CorrelatedGraph(graph);
            int actionKeyIndex = args.Length == 5 ? 4 : 2;
            Edge[] spanningTree = new Edge[0];
            switch (args[actionKeyIndex])
            {
                case "-k":
                    spanningTree = Kruscala.Run(graph);
                    break;
                case "-p":
                    spanningTree = Prima.Run(graph);
                    break;
                case "-b":
                    spanningTree = Boruvka.Run(graph).ToUndirectedTree();
                    break;
                case "-s":
                    Stopwatch timer = Stopwatch.StartNew();
                    spanningTree = Kruscala.Run(graph);
                    timer.Stop();
                    multiWriter.WriteLine("Kruscala: " + timer.ElapsedMilliseconds);
                    timer.Restart();
                    Prima.Run(graph);
                    timer.Stop();
                    multiWriter.WriteLine("Prima: " + timer.ElapsedMilliseconds);
                    timer.Restart();
                    Boruvka.Run(graph).ToUndirectedTree();
                    timer.Stop();
                    multiWriter.WriteLine("Boruvka " + timer.ElapsedMilliseconds);
                    break;
            }
            //int sum = 0;
            multiWriter.WriteLine(FormatEdgesArray(spanningTree, out int sum));
            //Graph graph1 = new Graph(graph.GetVertexCount());
            multiWriter.WriteLine("Weight of spanning tree:" + sum);

        }
        
        static string FormatEdgesArray(Edge[] array, out int sum)
        {
            StringBuilder sb = new StringBuilder();
            sum = 0;
            sb.Append('[');
            if (array.Length == 0)
            {
                sb.Append("]");
                return sb.ToString();
            }
            for (int i = 0; i < array.Length - 1; i++)
            {
                sum += array[i].weight;
                sb.AppendFormat("({0}, {1}, {2}), ", array[i].vi + 1, array[i].vj + 1, array[i].weight);
            }
            sum += array[array.Length - 1].weight;
            sb.AppendFormat("({0}, {1}, {2})]", array[array.Length - 1].vi + 1, array[array.Length - 1].vj + 1, array[array.Length - 1].weight);
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
        static public Graph CorrelatedGraph(Graph graph)
        {
            Graph correlatedGraph = new Graph(graph);
            if (!graph.IsDirected) return correlatedGraph;
            for (int i = 0; i < graph.AdjacentList.Length; i++)
            {
                foreach (var adjacentVertex in graph.AdjacentList[i])
                {
                    correlatedGraph.AdjacentList[adjacentVertex.Vj].Add(new AdjacentVertex(i, adjacentVertex.Weight));
                }

            }
            return correlatedGraph;
        }

    }
    internal static class Extensions
    {

        public static Edge[] ToUndirectedTree(this Edge[] edges)
        {
            Edge[] newEdges = new Edge[edges.Length / 2];
            List<KeyValuePair<int, int>> visited = new List<KeyValuePair<int, int>>();
            int j = 0;
            for (int i = 0; i < edges.Length; i++)
            {
                if (visited.FirstOrDefault(pair => pair.Key == edges[i].vj && pair.Value == edges[i].vi,new KeyValuePair<int,int>(-1,-1)).Value == -1)
                {
                    newEdges[j] = edges[i];
                    visited.Add(new KeyValuePair<int, int>(edges[i].vi, edges[i].vj));
                    j++;
                }
            }
            return newEdges;
        }

    }
}