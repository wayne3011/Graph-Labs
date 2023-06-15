using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLab.Components;

namespace Graph_Lab_9_2
{
    internal class BranchesAndBoundaries
    {
        Matrix adjecencyMatrix;
        Tree solutionsTree;
        public BranchesAndBoundaries(int[][] matrix)
        {
            adjecencyMatrix = new Matrix(matrix);
            solutionsTree = new Tree();
        }
        public void Run()
        {
            Matrix currentMatrix = adjecencyMatrix;
            while (currentMatrix.ColumnsCount != 2)
            {
                solutionsTree.primaryPhi = adjecencyMatrix.RowReduce() + adjecencyMatrix.ColumnReduce();
                Edge[] zerosMinimum = adjecencyMatrix.ZerosMinimum();
                Edge maxOption = zerosMinimum.Max()!;
                int phiNoOption = solutionsTree.primaryPhi + maxOption.weight;
            }
            

        }
    }
}
