using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
   public class CAlgoritmMinHeap : AHeapPriority
    {

       protected override void minHeapify(ref CHeap heap, int index)
        {
            int leftChild = getLeft(index);
            int rightChild = getRight(index);
            int minimal = index;
            
            if (leftChild <= heap.heapSize &&
               heap.nodes[leftChild].leaf.frequency < heap.nodes[index].leaf.frequency)
            {
                minimal = leftChild;
            }
            if (rightChild <= heap.heapSize &&
                heap.nodes[rightChild].leaf.frequency < heap.nodes[minimal].leaf.frequency)
            {
                minimal = rightChild;
            }

            if (minimal != index)
            {
                Helper.SwapNode(ref heap.nodes[index], ref heap.nodes[minimal]);
                minHeapify(ref  heap, minimal);
            }
        }


        public override void  buildMinHeap(ref CHeap heap)
        {
            heap.heapSize = heap.nodes.Length - 1;
            for (int i = heap.nodes.Length / 2; i >= 0; --i)
            {
                minHeapify(ref heap, i);
            }
        }

        public override CNode heapExtractMin(ref CHeap heap)
        {
            if (heap.heapSize < 0)
            {
                throw new Exception("пытается из пустой пирамиды взять элемент");
            }
            CNode min = heap.nodes[0];
            heap.nodes[0] = heap.nodes[heap.heapSize];
            heap.heapSize -= 1;
            minHeapify(ref heap, 0);
            return min;
        }

        public override void heapDecreaseFrequency(ref CHeap heap, int ind, int key)
        {
            if (key > heap.nodes[ind].leaf.frequency)
            {
                throw new Exception("новый ключ больше предыдущего");
            }
            heap.nodes[ind].leaf.frequency = key;
            while (ind > 0 && heap.nodes[getParent(ind)].leaf.frequency > heap.nodes[ind].leaf.frequency)
            {
                Helper.SwapNode(ref heap.nodes[ind], ref heap.nodes[getParent(ind)]);
                ind = getParent(ind);
            }
        }

        public override void minHeapInsert(ref CHeap heap, int freequency, int symbol)
        {
            heap.heapSize = heap.heapSize + 1;
            heap.nodes[heap.heapSize] = new CNode();
            heap.nodes[heap.heapSize].leaf = new CLeaf();
            heap.nodes[heap.heapSize].leaf.frequency = int.MaxValue;
            heap.nodes[heap.heapSize].leaf.symbol = symbol;
            heapDecreaseFrequency(ref heap, heap.heapSize, freequency);
        }
    
        public override void minHeapInsertNode(ref CHeap heap, ref  CNode node)
        {
            int freequency = node.leaf.frequency;
            heap.heapSize = heap.heapSize + 1;
            heap.nodes[heap.heapSize] = node;
            heap.nodes[heap.heapSize].leaf.frequency = int.MaxValue;
            heapDecreaseFrequency(ref heap, heap.heapSize, freequency);
        }
    }
}
