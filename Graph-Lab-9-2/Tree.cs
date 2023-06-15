using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_9_2
{
    public class TreeNode
    {
        public TreeNode? SelectOption;
        public TreeNode? NoSelectOption;
        public Edge CurrentEdge { get; set; }
        public bool select;
        internal Matrix CurrentAdjacecnyMatrix;
        public int phi { get; set; }
        public TreeNode()
        {

        }    
    }

    public class Tree
    {
        public TreeNode? root;
        public int primaryPhi { get; set; }
        public Tree()
        {
         
        }
    }
}
