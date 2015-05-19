using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
/// current version
namespace attemptSecondHuffman
{
    abstract class AWorkFlow
    {
        public enum Scenario
        {
            Compress,
            DeCompress
        };

        protected ICompressor m_compressor;
        protected IDecompressor m_decompressor;
        protected Scenario m_scenario; 
        private CNode rootTree;
        private string m_fileName;
        protected FileStream file1 = new FileStream("test.txt", FileMode.Create); //создаем файловый поток
        protected StreamWriter writer;
        protected AHeapPriority m_algorithmHeap;
        private CHeap m_heap = new CHeap();
        protected FileStream m_fileStream; 
        
        public AWorkFlow(AHeapPriority heapPriority, string fileName, Scenario scenario)
        {
            m_scenario = scenario;
            writer = new StreamWriter(file1); //создаем «потоковый писатель» и связываем его с файловым потоком
            m_algorithmHeap = heapPriority;
            m_fileName = fileName;
            for (int i = 0; i < m_heap.nodes.Count(); ++i)
            {
                m_heap.nodes[i] = new CNode();
                m_heap.nodes[i].leaf = new CLeaf();
            }
            m_heap.heapSize = -1;
            rootTree = new CNode();
        }
      
        public AWorkFlow(string fileName)
        {
            writer  = new StreamWriter(file1); 
            m_fileName = fileName;
        }

        public void run()
        {
            if (!File.Exists(m_fileName))
            {
                throw new Exception("File Not Found");
            }
            initScenario();
            if (m_scenario == Scenario.Compress)
            {
                compress();
            }
            else
            {
                deCompress();
            }
            writer.Close();  
        }

        private void compress(){
           Dictionary<int, string> weights = new Dictionary<int, string>();
            int []arr = new int[256];
            m_compressor.readByteByByte(ref m_fileName, ref arr);
            fillHeap(ref arr, ref m_heap);
            huffman(m_heap, ref rootTree);
            fillWeights(rootTree, "", ref weights);
            rootTree = null;
            m_compressor.createArchive(ref m_fileName, ref weights, ref m_heap);
        }


        private void deCompress()
        {
            if (!m_fileName.Contains(".lema"))
            {
                throw new Exception("Undefined file Format");
            }
            Dictionary<int, string> weights = new Dictionary<int, string>();
            m_fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(m_fileStream);
            m_decompressor.readHeap(ref m_heap, ref sr);
            huffman(m_heap, ref rootTree);

            
            fillWeights(rootTree, "", ref weights);
            String outFileName = Helper.getFileNameFromNameArchive(m_fileName);

            

                m_decompressor.transformArchiveToFile(ref rootTree, outFileName, ref sr);

             
            sr.Close();
            m_fileStream.Close();
        }
        /// <summary>
        /// Метод необходимо вызывать для инициализации сценария то есть разархивируем мы сейчас или архивируем
        /// </summary>
        abstract protected void initScenario();

        /// <summary>
        /// заполняем пирамиду
        /// </summary>
        /// <param name="arr"> массив встреаемости каждого ascii символа</param>
        /// <param name="heap">пирамида</param>
        abstract protected void fillHeap(ref int[] arr, ref CHeap heap);

        /// <summary>
        /// применяем алгоритм Хаффмана, строим здесь дерево из пирамиды
        /// </summary>
        /// <param name="m_heap">Пирамида</param>
        /// <param name="rootTree">указатель на корень дерева</param>
        abstract protected void huffman(CHeap m_heap, ref CNode rootTree);
        
        /// <summary>
        /// заполняем таблицу, ключ код символ значение соответствующий символу код Хаффмана 
        /// (по сути просто обхо дерева)
        /// </summary>
        /// <param name="node"> узел дерева</param>
        /// <param name="res"> тут зхранится текущий код</param>
        /// <param name="weights">словарь с весами</param>
        abstract protected void fillWeights(CNode node, string res, ref Dictionary<int, string> weights);
    }
}
