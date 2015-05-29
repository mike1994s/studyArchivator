using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace attemptSecondHuffman
{
    class CParallelWorkflow : AWorkFlow
    {
        const int COUNT_THREADS = 4;
        const string ADDITION_NAME = "{0}.part";
        private ProgressBar prBar;
        private ParallelForm pF;
        public CParallelWorkflow(AHeapPriority heapPriority, string fileName, AWorkFlow.Scenario scenario, ref ProgressBar progressBar1,  ParallelForm pForm)
            : base(heapPriority, fileName, scenario)
        {
            prBar = progressBar1;
            pF = pForm;
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

        private string helperMethod(string fileName)
        {
            String directoryPath = Path.GetDirectoryName(m_fileName);
            String onlyNameFile = Path.GetFileName(m_fileName);
            string newDirectory = directoryPath + onlyNameFile + Helper.getPostFixDirectory();
            Directory.CreateDirectory(@newDirectory);
            return newDirectory;
        }
        protected override void compress() {
            Dictionary<int, string> weights = new Dictionary<int, string>();
            int[] arr = new int[256];
            pF.Invoke(new Action(() =>
            {
                prBar.Maximum = 7;
                prBar.Value = 0;
            }));
            m_compressor.readByteByByte(ref m_fileName, ref arr);
            pF.Invoke(new Action(() =>
            {
                prBar.Value ++;
            }));
            Task<String> helperTask = Task.Factory.StartNew(() =>
            {
                return helperMethod(m_fileName);
            });
            Task taskDivider = Task.Factory.StartNew(() =>
            {
                divideFile(COUNT_THREADS);
            });
            huffmanAlgorithm.fillHeap(ref arr, ref m_heap);
            pF.Invoke(new Action(() =>
            {
                prBar.Value++;
            }));


            huffmanAlgorithm.huffman(ref m_heap, ref rootTree);
            pF.Invoke(new Action(() =>
            {
                prBar.Value++;
            }));


            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);
            pF.Invoke(new Action(() =>
            {
                prBar.Value++;
            }));
            rootTree = null;
            List<Task> tasks = new List<Task>();
           
            taskDivider.Wait();
            pF.Invoke(new Action(() =>
            {
                prBar.Value++;
            }));
            string newDirectory = helperTask.Result;
            for (int i = 0; i < COUNT_THREADS; ++i)
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


            pF.Invoke(new Action(() =>
            {
                prBar.Value++;
            }));

            for (int i = 0; i < 4; ++i)
            {
                String fileName = string.Format(m_fileName + ADDITION_NAME, i);
                File.Delete(fileName);
            }
            pF.Invoke(new Action(() =>
            {
                prBar.Value = prBar.Maximum;
            }));
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
           

            huffmanAlgorithm.huffman(ref heap, ref root);
          

            huffmanAlgorithm.fillWeights(root, "", ref weights);
           

            String outFileName = Helper.getFileNameFromNameArchive(file);
         

            m_decompressor.transformArchiveToFile(ref root, outFileName, ref sr);
         
            sr.Close();
            fileStream.Close();
            File.Delete(file);
        }

        private string getNameParts(string fileName)
        {
            String[] arr = fileName.Split('.');
            String newFileName = String.Empty;
            for (int i = 0; i < arr.Length - 1; ++i)
            {
                if (i == 0)
                {
                    newFileName += arr[i];
                }
                else
                {
                    newFileName += "." + arr[i];
                }
            }
            return newFileName;
        }

        private String getDearchivateFile(String fileName, int numPart)
        {
            String[] fileNames = fileName.Split('0');
            return fileNames[0];
        }

        protected override void deCompress() {
            string[] files = Directory.GetFiles(m_fileName);
            List<Task> tasks = new List<Task>();
            List<String> lStrings = new List<string>();
            pF.Invoke(new Action(() =>
            {
                prBar.Maximum = 2;
                prBar.Value = 0;
            }));

            foreach (string file in files)
            {
                string str = getNameParts(file);
                lStrings.Add(str);
                Task task = Task.Factory.StartNew(() =>
                {
                    dearchivate(file);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            pF.Invoke(new Action(() =>
            {
                prBar.Value ++;
            }));
            if (lStrings.Count() > 0)
            {
               String newFileName =  getDearchivateFile(lStrings[0], 0);
               Helper.MultipleFilesToSingleFile(lStrings, newFileName);
            }
            pF.Invoke(new Action(() =>
            {
                prBar.Value = prBar.Maximum;
            }));

        }
         
    }
   

}
