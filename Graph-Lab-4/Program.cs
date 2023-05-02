using GraphLab.Components;
using System.Text;
using GraphLab;
using Graph_Lab3.Components.OutputModels;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
            int actionKeyIndex = args.Length == 5 ? 4 : 2;
            Edge[] spanningTree = new Edge[0];
            switch(args[actionKeyIndex]) 
            {
                case "-k":
                    spanningTree = graph.Kruscala();
                    break;
                case "-p":
                    spanningTree = graph.Prima();
                    break;
                case "-b":
                    spanningTree = graph.Boruvka();
                    break;
                case "-s":
                    Stopwatch timer = Stopwatch.StartNew();
                    graph.Kruscala();
                    timer.Stop();
                    multiWriter.WriteLine("Kruscala: " + timer.ElapsedMilliseconds);
                    timer.Restart();
                    graph.Prima();
                    timer.Stop();
                    multiWriter.WriteLine("Prima: " + timer.ElapsedMilliseconds);
                    timer.Restart();
                    graph.Boruvka();
                    timer.Stop();
                    multiWriter.WriteLine("Boruvka " + timer.ElapsedMilliseconds);
                    break;
            }
            //int sum = 0;
            multiWriter.WriteLine(FormatEdgesArray(spanningTree,out int sum));
            multiWriter.WriteLine("Weight of spanning tree:"+ (sum/2));

        }
        static string FormatEdgesArray(Edge[] array,out int sum)
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
                sb.AppendFormat("({0}, {1}, {2}), ", array[i].vi+1, array[i].vj+1, array[i].weight);
            }
            sum += array[array.Length-1].weight;
            sb.AppendFormat("({0}, {1}, {2})]", array[array.Length-1].vi+1, array[array.Length-1].vj+1, array[array.Length-1].weight);
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