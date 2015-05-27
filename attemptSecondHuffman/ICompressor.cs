using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attemptSecondHuffman
{
    public interface ICompressor
    {
        /// <summary>
        /// Создаем Архив
        /// </summary>
        /// <param name="m_fileName">Имя Файла котрый архивируем</param>
        /// <param name="weights">словарь с весами</param>
        /// <param name="m_heap">Пирамида котороую записываем в начало архива</param>
        void createArchive(ref String m_fileName, ref Dictionary<int, string> weights, ref CHeap m_heap);


        /// <summary>
        /// читаем файл по одному байту
        /// </summary>
        /// <param name="m_fileName">файл (путь и имя)</param>
        /// <param name="arr">массив встреаемости каждого ascii символа<</param>
        void readByteByByte(ref String m_fileName, ref int[] arr);
    }
}
