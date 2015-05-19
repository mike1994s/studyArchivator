using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public class CNode
    {
        public CNode(CNode other)
        {
            this.leaf = new CLeaf(other.leaf);

            this.left = this.left;
       
            this.right = other.right;

        }
        public CNode()
        {

        }
        public CLeaf leaf; 
        public CNode right { set; get; }
        public CNode left { set; get; }
    }
}
