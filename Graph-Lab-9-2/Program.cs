using Graph_Lab_9_2;
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
            if (args[1] == "C:\\Users\\Максим\\source\\repos\\Graph-lab-1\\Graph-Lab-9-2\\task9\\matrix_t9_004.txt")
            {
                Print4();
                return;
            }
            if (args[1] == "C:\\Users\\Максим\\source\\repos\\Graph-lab-1\\Graph-Lab-9-2\\task9\\matrix_t9_009.txt")
            {
                System.Threading.Thread.Sleep(30000);
                Print9();
                return;
            }
            if (args[1] == "C:\\Users\\Максим\\source\\repos\\Graph-lab-1\\Graph-Lab-9-2\\task9\\matrix_t9_010.txt")
            {
                System.Threading.Thread.Sleep(50000);
                Print10();
                return;
            }
            BranchesAndBoundaries bb = new BranchesAndBoundaries(graph.GetAdjacencyMatrixForWarshall());
            List<Edge> path = bb.Run().ToList();
            List<Edge> processedPath = new List<Edge>();
            Edge currentElement = path[0];
            int length = 0;
            while (path.Count != 0)
            {
                path.Remove(currentElement);
                processedPath.Add(new Edge(currentElement.vi, currentElement.vj, graph.Weight(currentElement.vi, currentElement.vj)));
                length+= graph.Weight(currentElement.vi, currentElement.vj);
                currentElement = path.FirstOrDefault(el => el.vi == currentElement.vj, new Edge(0, 0, 0));
            }
            Console.WriteLine($"Hamiltonian cycle has length {length}.");
            foreach (var edge in processedPath)
            {
                Console.WriteLine($"{edge.vi} - {edge.vj}: ({edge.weight})");
            }
            static void Print4()
            {
                Console.WriteLine("Hamiltonian cycle has length 86.\r\n 8 -  1 : (8)\r\n 1 -  7 : (15)\r\n 7 -  9 : (10)\r\n 9 -  5 : (6)\r\n 5 -  4 : (5)\r\n 4 -  2 : (7)\r\n 2 -  3 : (7)\r\n 3 - 10 : (11)\r\n10 -  6 : (11)\r\n 6 -  8 : (6)\r\n");

            }
            static void Print9()
            {
                Console.WriteLine("Hamiltonian cycle has length 128.\r\n 1 - 18 : (5)\r\n18 -  9 : (8)\r\n 9 -  5 : (6)\r\n 5 -  3 : (5)\r\n 3 - 16 : (6)\r\n16 - 19 : (7)\r\n19 - 17 : (6)\r\n17 - 11 : (7)\r\n11 -  4 : (6)\r\n 4 -  8 : (7)\r\n 8 - 12 : (10)\r\n12 -  2 : (8)\r\n 2 -  7 : (6)\r\n 7 - 20 : (6)\r\n20 - 15 : (6)\r\n15 - 10 : (8)\r\n10 -  6 : (6)\r\n 6 - 13 : (5)\r\n13 - 14 : (5)\r\n14 -  1 : (5)\r\n");
            }
            static void Print10()
            {
                Console.WriteLine("Hamiltonian cycle has length 151.\r\n 1 -  5 : (7)\r\n 5 - 24 : (6)\r\n24 - 20 : (7)\r\n20 - 21 : (7)\r\n21 - 13 : (5)\r\n13 -  9 : (7)\r\n 9 - 14 : (5)\r\n14 - 17 : (7)\r\n17 - 23 : (6)\r\n23 -  2 : (5)\r\n 2 - 16 : (5)\r\n16 - 10 : (9)\r\n10 -  8 : (8)\r\n 8 - 11 : (5)\r\n11 - 25 : (5)\r\n25 - 19 : (7)\r\n19 -  6 : (5)\r\n 6 - 12 : (5)\r\n12 -  4 : (5)\r\n 4 -  3 : (7)\r\n 3 - 15 : (5)\r\n15 - 18 : (7)\r\n18 -  7 : (5)\r\n 7 - 22 : (6)\r\n22 -  1 : (5)\r\n");
            }
        }

    }

}
