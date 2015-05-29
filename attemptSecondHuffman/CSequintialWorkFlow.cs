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
    class CSequintialWorkFlow : AWorkFlow
    {
        private ProgressBar prBar;
        public CSequintialWorkFlow(AHeapPriority heapPriority, string fileName, AWorkFlow.Scenario scenario, ref ProgressBar progressBar1)
            : base(heapPriority, fileName, scenario)
        {
            prBar = progressBar1;
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

        protected override void compress()
        {
            prBar.Maximum = 5;
            prBar.Value= 0;

            Dictionary<int, string> weights = new Dictionary<int, string>();
            int[] arr = new int[256];
            m_compressor.readByteByByte(ref m_fileName, ref arr);
            prBar.Value++;

            huffmanAlgorithm.fillHeap(ref arr, ref m_heap);
            prBar.Value++;

            
            huffmanAlgorithm.huffman(ref m_heap, ref rootTree);
            prBar.Value++;

            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);
            prBar.Value++;

            rootTree = null;
            m_compressor.createArchive( m_fileName,  weights, m_heap, "");
            prBar.Value = prBar.Maximum;
        }

        protected override void deCompress()
        {
            prBar.Maximum = 5;
            prBar.Value = 0;
            if (!m_fileName.Contains(".lema"))
            {
                throw new Exception("Undefined file Format");
            }
            Dictionary<int, string> weights = new Dictionary<int, string>();
            m_fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(m_fileStream);
            m_decompressor.readHeap(ref m_heap, ref sr);
            prBar.Value++;

            huffmanAlgorithm.huffman(ref m_heap, ref rootTree);
            prBar.Value++;


            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);
            prBar.Value++;


            String outFileName = Helper.getFileNameFromNameArchive(m_fileName);
            prBar.Value++;
            
            m_decompressor.transformArchiveToFile(ref rootTree, outFileName, ref sr );
            sr.Close();
            m_fileStream.Close();
            prBar.Value = prBar.Maximum;
        }
    }
}