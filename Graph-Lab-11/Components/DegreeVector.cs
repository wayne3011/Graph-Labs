using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLab.Components
{
    /// <summary>
    /// A class that stores a vector of incoming and outgoing degrees of vertices 
    /// </summary>
    /// <remarks>
    /// Note: For an undirected graph, the outgoing and incoming vector
    /// </remarks>
    internal class DegreeVector
    {
        private readonly bool _isDirected;
        private int[] _outgoingVector;
        private int[] _incomingVector;
        public DegreeVector(bool isDirected, int vertexCount)
        {
            _isDirected = isDirected;
            _outgoingVector = new int[vertexCount];
            if(_isDirected)_incomingVector = new int[vertexCount];
        }
        internal void SetOutgoingVectorDegree(int vertexNumber, int degree)
        {
            _outgoingVector[vertexNumber] = degree;
        }
        internal void SetIncomingVectorDegree(int vertexNumber, int degree)
        {
            if (!_isDirected) _outgoingVector[vertexNumber] = degree;
            else _incomingVector[vertexNumber] = degree;
        }
        public int[] GetIncomingVector()
        {
            if(_isDirected) return _incomingVector;
            else return _outgoingVector;
        }
        public int[] GetOutgoingVector()
        {
            return _outgoingVector;
        }
        
        
    }
}
