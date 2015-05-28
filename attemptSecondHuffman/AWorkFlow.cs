using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

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
        protected CNode rootTree;
        protected string m_fileName;
        protected FileStream file1 = new FileStream("test.txt", FileMode.Create); //создаем файловый поток
        protected StreamWriter writer;
        protected CHeap m_heap = new CHeap();
        protected IHuffman huffmanAlgorithm;
        protected FileStream m_fileStream;
        public AWorkFlow(AHeapPriority heapPriority, string fileName, Scenario scenario)
        {
            m_scenario = scenario;
            writer = new StreamWriter(file1); //создаем «потоковый писатель» и связываем его с файловым потоком
            huffmanAlgorithm = new CSequintialHuffman(heapPriority);
            m_fileName = fileName;
            Helper.initHeap(ref m_heap);
            rootTree = new CNode();
        }
        
        public AWorkFlow(string fileName)
        {
            writer  = new StreamWriter(file1); 
            m_fileName = fileName;
        }

        public String run()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (!File.Exists(m_fileName) && !Directory.Exists(m_fileName))
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
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            writer.Close();
            return elapsedTime;
        }
        /// <summary>
        /// сжимает файл в архив
        /// </summary>
        abstract protected void compress();


        /// <summary>
        /// Получает файл из архива
        /// </summary>
        abstract protected void deCompress();


        /// <summary>
        /// Метод необходимо вызывать для инициализации сценария то есть разархивируем мы сейчас или архивируем
        /// </summary>
        abstract protected void initScenario();
    }
}
