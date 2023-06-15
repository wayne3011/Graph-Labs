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
        List<TreeNode> children { get; set; } = new List<TreeNode>();
        public int phi { get; set; }
        Dictionary<Edge, bool> options { get; set; } = new Dictionary<Edge, bool>();
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
