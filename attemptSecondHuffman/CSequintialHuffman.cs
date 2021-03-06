﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public class CSequintialHuffman : IHuffman
    {
        private AHeapPriority m_algorithmHeap;

        public CSequintialHuffman(AHeapPriority heapAglorithm)
        {
            m_algorithmHeap = heapAglorithm;
        }
     /**
     *  заполняем пирамиду 
     * 
     * */
        public void fillHeap(ref int[] arr, ref CHeap heap)
        {
            for (int i = 0; i < 256; ++i)
            {
                if (arr[i] != 0)
                    m_algorithmHeap.minHeapInsert(ref heap, arr[i], i); // вставляем символ и частоту в кучу
            }
        }

        /**
        *  заполняем вспомогательную таблицу которая сожержит символ и код
        * 
        * 
        * */
        public void fillWeights(CNode node, string res, ref Dictionary<int, string> weights)
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
        public void huffman(ref CHeap m_heap, ref CNode rootTree)
        {
            int n = m_heap.heapSize;
            CHeap heap = new CHeap(m_heap);
            int count = 0;
            for (int i = 0; i <= n - 1; ++i)
            {
                CNode left = m_algorithmHeap.heapExtractMin(ref heap);
                CNode right = m_algorithmHeap.heapExtractMin(ref heap);
                int freq = left.leaf.frequency + right.leaf.frequency;
                CNode newNode = new CNode();
                count++;
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
