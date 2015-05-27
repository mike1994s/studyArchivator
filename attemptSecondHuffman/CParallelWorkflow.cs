using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace attemptSecondHuffman
{
    class CParallelWorkflow : AWorkFlow
    {
        const string ADDITION_NAME = "{0}.part";
        public CParallelWorkflow(AHeapPriority heapPriority, string fileName, AWorkFlow.Scenario scenario)
            : base(heapPriority, fileName, scenario)
        {

        }
        protected override void initScenario()
        {
            if (m_scenario == Scenario.Compress)
            {
                m_compressor = new CParallelCompressor(4, ADDITION_NAME);
            }
            else
            {
                m_decompressor = new CSequintialDeCompressor();
            }
        }

        private void divideFile(int countParts)
        {
            using (FileStream fs = new FileStream(m_fileName, FileMode.Open, FileAccess.Read))
            {
                long partSz = fs.Length / countParts; // Размер одной части
                byte[] buff;
                bool mod = fs.Length % countParts == 0; // Все части одного размера
                for (int i = 0; i < countParts; i++)
                {
                    using (FileStream pStream = new FileStream(string.Format(m_fileName + ADDITION_NAME, i), FileMode.Create, FileAccess.Write))
                    {
                        buff = new byte[i == countParts - 1 && !mod ? fs.Length - (countParts - 1) * partSz : partSz];
                        fs.Read(buff, 0, buff.Length);
                        pStream.Write(buff, 0, buff.Length);
                    }
                    buff = null;
                }
            }
        }
        protected override void compress() {
            Dictionary<int, string> weights = new Dictionary<int, string>();
            int[] arr = new int[256];
            m_compressor.readByteByByte(ref m_fileName, ref arr);
            Task taskDivider = Task.Factory.StartNew(() =>
            {
                divideFile(4);
            });
            huffmanAlgorithm.fillHeap(ref arr, ref m_heap);
            huffmanAlgorithm.huffman(m_heap, ref rootTree);
            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);
            rootTree = null;
            List<Task> tasks = new List<Task>();
            String directoryPath = Path.GetDirectoryName(m_fileName);
            String onlyNameFile = Path.GetFileName(m_fileName);
            string newDirectory = directoryPath + onlyNameFile + Helper.getPostFixDirectory();
            Directory.CreateDirectory(@newDirectory);
            taskDivider.Wait();
            for (int i = 0; i < 4; ++i)
            {
                String file = m_fileName;
                String fileName = string.Format(file + ADDITION_NAME, i);
                Task task = Task.Factory.StartNew(() =>
                {
                    m_compressor.createArchive(fileName, weights, m_heap, newDirectory);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            for (int i = 0; i < 4; ++i)
            {
                String fileName = string.Format(m_fileName + ADDITION_NAME, i);
                File.Delete(fileName);
            }
        }

        private void dearchivate(String file)
        {
            Dictionary<int, string> weights = new Dictionary<int, string>();
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fileStream);
            CHeap heap= new CHeap();
            Helper.initHeap(ref heap);
            CNode root = new CNode();
            m_decompressor.readHeap(ref heap, ref sr);

            huffmanAlgorithm.huffman(m_heap, ref root);

            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);

            String outFileName = Helper.getFileNameFromNameArchive(file);
            m_decompressor.transformArchiveToFile(ref root, outFileName, ref sr);
            sr.Close();
            m_fileStream.Close();
        }
        protected override void deCompress() {
            string[] files = Directory.GetFiles(m_fileName);
            List<Task> tasks = new List<Task>();
            foreach (string file in files)
            {
                dearchivate(file);
            }
           
        }
    }
   

}
