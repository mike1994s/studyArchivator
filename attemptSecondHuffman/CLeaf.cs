using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public class CLeaf 
    {
        public CLeaf() { }
        public CLeaf(CLeaf other)
        {
            this.frequency = other.frequency;
            this.symbol = other.symbol;
        }
        public int symbol { set; get; }
        public int frequency { set; get; }
    }
}
