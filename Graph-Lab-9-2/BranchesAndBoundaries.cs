using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLab.Components;
using Microsoft.VisualBasic.FileIO;

namespace Graph_Lab_9_2
{
    internal class BranchesAndBoundaries
    {
        Matrix adjecencyMatrix;
        Tree solutionsTree;
        Edge finalEdge;
        int minimalPhi = 0;
        public BranchesAndBoundaries(int[][] matrix)
        {
            adjecencyMatrix = new Matrix(matrix);
            solutionsTree = new Tree();
        }
        public Edge[] Run()
        {
            Matrix currentMatrix = adjecencyMatrix;
            solutionsTree.primaryPhi = adjecencyMatrix.RowReduce() + adjecencyMatrix.ColumnReduce();
            solutionsTree.root = new TreeNode();
            solutionsTree.root.phi = solutionsTree.primaryPhi;
            solutionsTree.root.CurrentAdjacecnyMatrix = adjecencyMatrix;
            PriorityQueue<TreeNode, int> notBranched = new PriorityQueue<TreeNode, int>();
            TreeNode ParentNode = solutionsTree.root;
            while (true)
            {
                Console.WriteLine("Processing: " + (ParentNode.select ? "no" : "") + ParentNode.CurrentEdge);
                ParentNode.CurrentAdjacecnyMatrix.PrintMatrix();
                Console.WriteLine();
                Edge option = ParentNode.CurrentAdjacecnyMatrix.ZerosMinimum().Max()!;

                Console.WriteLine("Get From zeros: " + option);
                //select option
                TreeNode SelectedOption = new TreeNode();
                SelectedOption.CurrentAdjacecnyMatrix = ParentNode.CurrentAdjacecnyMatrix.SelectOption(option);
                Console.WriteLine("Select option and remove column and rows: ");
                SelectedOption.CurrentAdjacecnyMatrix.PrintMatrix();
                SelectedOption.phi = ParentNode.phi == int.MaxValue ? int.MaxValue : ParentNode.phi + SelectedOption.CurrentAdjacecnyMatrix.RowReduce() + SelectedOption.CurrentAdjacecnyMatrix.ColumnReduce();
                Console.WriteLine("Reduced Matrix:");
                SelectedOption.CurrentAdjacecnyMatrix.PrintMatrix();
                Console.WriteLine("If take option: " + option + " = " + SelectedOption.phi);
                SelectedOption.select = true;
                SelectedOption.CurrentEdge = option;
                ParentNode.SelectOption = SelectedOption;
                if (SelectedOption.CurrentAdjacecnyMatrix.ColumnsCount == 1)
                {
                    finalEdge = SelectedOption.CurrentEdge;
                    minimalPhi = SelectedOption.phi;
                    List<Edge> edges = GetResult();
               
                    edges.Add(SelectedOption.CurrentAdjacecnyMatrix.GetLastEdge());
                    if ( CheckCycle(edges)) return edges.ToArray();
                    goto skip;

                }
                notBranched.Enqueue(SelectedOption, SelectedOption.phi);
                //dont select option
                TreeNode NotSelectedOption = new TreeNode();
                NotSelectedOption.CurrentAdjacecnyMatrix = ParentNode.CurrentAdjacecnyMatrix;
                NotSelectedOption.phi = ParentNode.phi == int.MaxValue || option.weight == int.MaxValue ? int.MaxValue : ParentNode.phi + option.weight;
                Console.WriteLine("If didnt take option: " + option + " = " + NotSelectedOption.phi);
                NotSelectedOption.select = false;
                NotSelectedOption.CurrentEdge = option;
                ParentNode.NoSelectOption = NotSelectedOption;
                notBranched.Enqueue(NotSelectedOption, NotSelectedOption.phi);
                skip:
                TreeNode minimumPhi = notBranched.Dequeue();

                if (minimumPhi.select) ParentNode = minimumPhi;
                else
                {
                    minimumPhi.CurrentAdjacecnyMatrix.BanEdge(minimumPhi.CurrentEdge);
                    minimumPhi.CurrentAdjacecnyMatrix.RowReduce();
                    minimumPhi.CurrentAdjacecnyMatrix.ColumnReduce();
                    ParentNode = minimumPhi;
                }

            }
        }
        private List<Edge> GetResult()
        {
            List<Edge> path = new List<Edge>();
            Search(path, solutionsTree.root!);
            return path;
        }
        private bool CheckCycle(List<Edge> curpath)
        {
            List<Edge> path = new List<Edge>(curpath);
            Edge currentElement = path[0];
            while (path.Count != 1)
            {
                if (currentElement.vi == 0 && currentElement.vj == 6)
                {
                    Console.WriteLine();
                }
                path.Remove(currentElement);
                //processedPath.Add(new Edge(currentElement.vi, currentElement.vj, graph.Weight(currentElement.vi, currentElement.vj)));
                currentElement = path.FirstOrDefault(el => el.vi == currentElement.vj || el.vj == currentElement.vj, new Edge(0, 0, 0));
                if (currentElement.vi == 0 && currentElement.vj == 0 && currentElement.weight == 0) return false;
            }
            return true;
        }
        private bool Search(List<Edge> path, TreeNode currentNode)
        {
            if (currentNode.NoSelectOption == null)
            {
                if (currentNode.SelectOption != null )
                {
                    path.Add(currentNode.SelectOption.CurrentEdge);
                    if (currentNode.select) path.Add(currentNode.CurrentEdge);
                    return true;
                }
                else
                {
                    return false;
                }
            }


            if (Search(path, currentNode.SelectOption) || Search(path, currentNode.NoSelectOption)) { if (currentNode.select) path.Add(currentNode.CurrentEdge); return true; }
            return false;
        }
    }
}
