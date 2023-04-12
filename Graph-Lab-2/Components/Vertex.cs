using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphLab.Components
{
    internal class Vertex
    {
        public int Number;
        public int Mark = 0;
        public Vertex(int number)
        {
            Number = number;
        }
        public Vertex(int number, int mark) : this(number)
        {
            Mark = mark;
        }
    }
}
