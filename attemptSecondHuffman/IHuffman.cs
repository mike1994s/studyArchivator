using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public interface IHuffman
    {

        /// <summary>
        /// заполняем пирамиду
        /// </summary>
        /// <param name="arr"> массив встреаемости каждого ascii символа</param>
        /// <param name="heap">пирамида</param>
        void fillHeap(ref int[] arr, ref CHeap heap);

        /// <summary>
        /// применяем алгоритм Хаффмана, строим здесь дерево из пирамиды
        /// </summary>
        /// <param name="m_heap">Пирамида</param>
        /// <param name="rootTree">указатель на корень дерева</param>
        void huffman(ref CHeap m_heap, ref CNode rootTree);

        /// <summary>
        /// заполняем таблицу, ключ код символ значение соответствующий символу код Хаффмана 
        /// (по сути просто обхо дерева)
        /// </summary>
        /// <param name="node"> узел дерева</param>
        /// <param name="res"> тут зхранится текущий код</param>
        /// <param name="weights">словарь с весами</param>
        void fillWeights(CNode node, string res, ref Dictionary<int, string> weights);
    }
}
