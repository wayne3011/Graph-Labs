using GraphLab.Components;
using GraphLab;
using System.Text;

namespace GraphLab1
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
                    Console.WriteLine("{            ЛАБАРАТОРНАЯ РАБОТА № 1             }");
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
            DegreeVector degreeVector = graph.GetDegreeVector();            
            if (graph.IsDirected)
            {               
                if(FileOutput)
                {
                    stream.Write("deg+ = ");
                    stream.WriteLine(FormatOutputArray(degreeVector.GetIncomingVector()));
                    stream.Write("deg- = ");
                    stream.WriteLine(FormatOutputArray(degreeVector.GetOutgoingVector()));
                }
                Console.Write("deg+ = ");
                Console.WriteLine(FormatOutputArray(degreeVector.GetIncomingVector()));
                Console.Write("deg- = ");
                Console.WriteLine(FormatOutputArray(degreeVector.GetOutgoingVector()));
            }
            else
            {
                if (FileOutput)
                {
                    stream.Write("deg = ");
                    stream.WriteLine(FormatOutputArray(degreeVector.GetOutgoingVector()));
                }
                Console.Write("deg = ");
                Console.WriteLine(FormatOutputArray(degreeVector.GetOutgoingVector()));
            }
            int[][] FloydWarshallMatrix = graph.FloydWarshallAlgorithm();
            if (FileOutput)
            {
                stream.WriteLine("Distancies:");
                stream.Write(FormatOutputMatrix(FloydWarshallMatrix));
            }
            Console.WriteLine("Distancies:");
            Console.Write(FormatOutputMatrix(FloydWarshallMatrix));
            if (graph.IsDirected) {
                stream.Flush();
                return;
            }
            if (FileOutput)
            {
                stream.WriteLine("Eccentricity:");
                stream.WriteLine(FormatOutputArray(Graph.GetEccentricity(FloydWarshallMatrix)));
                stream.WriteLine("D = {0}", graph.GetDiameter());
                stream.WriteLine("R = {0}", graph.GetRadius());
                stream.Write("Z = "); stream.WriteLine(FormatOutputArray(graph.GetCentralVertices()));
                stream.Write("P = "); stream.WriteLine(FormatOutputArray(graph.GetPeripheralVertices()));
            }
            Console.WriteLine("Eccentricity:");
            Console.WriteLine(FormatOutputArray(Graph.GetEccentricity(FloydWarshallMatrix)));
            Console.WriteLine("D = {0}",graph.GetDiameter());
            Console.WriteLine("R = {0}", graph.GetRadius());
            int[] centralVertices = graph.GetCentralVertices();
            int[] peripheralVertices = graph.GetPeripheralVertices();
            for (int i = 0; i < centralVertices.Length; i++)
            {
                centralVertices[i]++;
            }
            for (int i = 0; i < peripheralVertices.Length; i++)
            {
                peripheralVertices[i]++;
            }
            Console.Write("Z = "); Console.WriteLine(FormatOutputArray(centralVertices));
            Console.Write("P = "); Console.WriteLine(FormatOutputArray(peripheralVertices));
            stream.Flush();
        }
        static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int  j = 0;  j < matrix[i].Length;  j++)
                {
                    if (matrix[i][j] == int.MaxValue) Console.Write("∞ ",Encoding.UTF8);
                    else Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void PrintArray(object[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }
        static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }
        static string FormatOutputArray(int[] array) 
        {
            StringBuilder result = new StringBuilder("[",array.Length*2);
            for (int i = 0; i < array.Length-1; i++)
            {
                result.AppendFormat("{0}, ",array[i]);
            }
            result.AppendFormat("{0}]", array[array.Length-1]);
            return result.ToString();
        }
        static string FormatOutputMatrix(int[][] matrix)
        {
            StringBuilder sb = new StringBuilder(matrix.Length * matrix.Length);
            for (int i = 0; i < matrix.Length; i++)
            {
                sb.Append("[");
                for (int j = 0; j < matrix[i].Length-1; j++)
                {
                    if (matrix[i][j] < 10) sb.AppendFormat(" {0} ", matrix[i][j]);
                    else if(matrix[i][j]!=int.MaxValue) sb.AppendFormat("{0} ", matrix[i][j]);
                    else sb.Append(" ∞ ");
                }
                if (matrix[i][matrix.Length - 1] < 10) sb.AppendFormat(" {0}]\n", matrix[i][matrix.Length - 1]);
                else if (matrix[i][matrix.Length - 1] != int.MaxValue) sb.AppendFormat("{0}]\n", matrix[i][matrix.Length - 1]);
                else sb.AppendFormat(" ∞]\n");
            }
            return sb.ToString();
        }


    }
}