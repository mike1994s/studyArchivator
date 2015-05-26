using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace attemptSecondHuffman
{
    class CParallelWorkflow : AWorkFlow
    {
        public CParallelWorkflow(AHeapPriority heapPriority, string fileName, AWorkFlow.Scenario scenario, List<IObservebale> observs)
            : base(heapPriority, fileName, scenario, observs)
        {

        }
        protected override void initScenario()
        {
            if (m_scenario == Scenario.Compress)
            {
                m_compressor = new CSequintialCompressor();
            }
            else
            {
                m_decompressor = new CSequintialDeCompressor();
            }
        }
        /**
         *  заполняем пирамиду 
         * 
         * */
        protected override void fillHeap(ref int[] arr, ref CHeap heap)
        {
            
        }

        /**
        *  заполняем вспомогательную таблицу которая сожержит символ и код
        * 
        * 
        * */
        protected override void fillWeights(CNode node, string res, ref Dictionary<int, string> weights)
        {
            if (node != null)
            {
                if (node.left == null && node.right == null)
                {
                    weights.Add(node.leaf.symbol, res);
                }
                fillWeights(node.left, res + "0", ref weights);
                fillWeights(node.right, res + "1", ref weights);
            }
        }
        /**
         *  строим дерево Хаффмана. Листы в этом дереве являются наши символы
         *  а путь до листьев и является кодом этого символа( есил идем влево пишем 0 , вправо 1)
         * 
         * */
        protected override void huffman(CHeap m_heap, ref CNode rootTree)
        {
            int n = m_heap.heapSize;
            CHeap heap = new CHeap(m_heap);
            for (int i = 0; i <= n - 1; ++i)
            {
                CNode left = m_algorithmHeap.heapExtractMin(ref heap);
                CNode right = m_algorithmHeap.heapExtractMin(ref heap);
                int freq = left.leaf.frequency + right.leaf.frequency;
                CNode newNode = new CNode();
                newNode.left = left;
                newNode.right = right;
                newNode.leaf = new CLeaf();
                newNode.leaf.frequency = freq;
                m_algorithmHeap.minHeapInsertNode(ref heap, ref newNode);
                rootTree = newNode;
            }
        }
    }
}
