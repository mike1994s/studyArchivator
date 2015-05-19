using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public class CHeap
    {
        public CHeap() {
            nodes = new CNode[256];
        }
        public CHeap(CHeap other)
        {
            this.heapSize = other.heapSize;
            this.nodes = new CNode[256];
            for (int i = 0; i < 256; ++i)
            {
                this.nodes[i] = new CNode(other.nodes[i]);
            }
        }
        public CNode[] nodes { get; set;}
        public int heapSize  { get; set;}
    }
}
