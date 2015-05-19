using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace attemptSecondHuffman
{
    public interface IDecompressor
    {
        /// <summary>
        /// считываем пирамиду из файла 
        /// </summary>
        /// <param name="m_heap">пирамида</param>
        /// <param name="readerArchive">откуда читаем</param>
        void readHeap(ref CHeap m_heap, ref  StreamReader readerArchive);

        /// <summary>
        /// из архива создаем файл
        /// </summary>
        /// <param name="rootTree"> указатель на корень дерева</param>
        /// <param name="outFileName">Название выходного файла</param>
        /// <param name="readerArchive"> ссылка на поток архива</param>
        void transformArchiveToFile(ref CNode rootTree, String outFileName, ref  StreamReader readerArchive);
    }
}
