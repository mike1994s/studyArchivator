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

        public CSequintialWorkFlow(AHeapPriority heapPriority, string fileName, AWorkFlow.Scenario scenario)
            : base(heapPriority, fileName, scenario)
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

        protected override void compress()
        {
            Dictionary<int, string> weights = new Dictionary<int, string>();
            int[] arr = new int[256];
            m_compressor.readByteByByte(ref m_fileName, ref arr);
            huffmanAlgorithm.fillHeap(ref arr, ref m_heap);
            huffmanAlgorithm.huffman(m_heap, ref rootTree);
            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);
            rootTree = null;
            m_compressor.createArchive(ref m_fileName, ref weights, ref m_heap);
        }

        protected override void deCompress()
        {
            if (!m_fileName.Contains(".lema"))
            {
                throw new Exception("Undefined file Format");
            }
            Dictionary<int, string> weights = new Dictionary<int, string>();
            m_fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(m_fileStream);
            m_decompressor.readHeap(ref m_heap, ref sr);

            huffmanAlgorithm.huffman(m_heap, ref rootTree);

            huffmanAlgorithm.fillWeights(rootTree, "", ref weights);

            String outFileName = Helper.getFileNameFromNameArchive(m_fileName);
            m_decompressor.transformArchiveToFile(ref rootTree, outFileName, ref sr );
            sr.Close();
            m_fileStream.Close();
        }
    }
}