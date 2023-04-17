using GraphLab.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab3.Components.OutputModels
{
    internal class BridgesAndHingesResult
    {
        public Edge[] Bridges;
        public int[] Hinges;
        public BridgesAndHingesResult()
        {
            Bridges = new Edge[0];
            Hinges = new int[0];
        }
    }
}
