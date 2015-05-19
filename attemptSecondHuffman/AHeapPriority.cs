using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public abstract class AHeapPriority
    {
        public int getParent(int index)
        {
            return (index / 2);
        }
        public int getLeft(int index)
        {
            return 2 * index;
        }
        public int getRight(int index)
        {
            return 2 * index + 1;
        }
        abstract protected void minHeapify(ref CHeap heap, int index);
        abstract public void buildMinHeap(ref CHeap heap);
        abstract public CNode heapExtractMin(ref CHeap heap);
        abstract public void heapDecreaseFrequency(ref CHeap heap, int ind, int key);
        abstract public void minHeapInsert(ref CHeap heap, int freequency, int symbol);
        abstract public void minHeapInsertNode(ref CHeap heap, ref  CNode node);
    }
}
