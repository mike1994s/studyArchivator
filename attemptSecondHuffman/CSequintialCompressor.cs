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
    public class CSequintialCompressor : ICompressor
    {
        private FileStream m_fileStream;
        public void createArchive(ref String m_fileName, ref Dictionary<int, string> weights, ref CHeap m_heap)
        {
            FileStream archive = new FileStream(m_fileName + ".lema", FileMode.Create); 
            StreamWriter writerArchive = new StreamWriter(archive);
            WriteTreeInArchive(ref m_heap, ref writerArchive);
            m_fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            List<String> a = new List<string>();
            int countSymbols = 0;
            
            
            int rest = getCountExtraByte(ref m_fileName, ref weights, a, ref m_fileStream,ref countSymbols);
            writerArchive.BaseStream.WriteByte((byte)rest);
            byte[] bytes = BitConverter.GetBytes(countSymbols);
            writerArchive.BaseStream.Write(bytes, 0, bytes.Length);
            //progressBar.Value = 0;
            //progressBar.Maximum = countSymbols;
            transform(ref m_fileName, ref weights, ref writerArchive, rest);
            //progressBar.Value = progressBar.Maximum;
            writerArchive.Close();
            archive.Close();
        }
        private String getEightSymbols(ref String eight)
        {
            String str = String.Empty;
            for (int i = 0; i < 8; ++i)
            {
                str += eight[i];
            }
            String tmpValue = String.Empty;
            for (int i = 8; i < eight.Length; ++i)
            {
                tmpValue += eight[i];
            }
            eight = tmpValue;
            return str;
        }

        private int getCountExtraByte(ref String m_fileName, ref Dictionary<int, string> weights, List<String> arr, ref FileStream fStream, ref int countSymbols)
        {
            StreamReader sr = new StreamReader(fStream);
            string lines = string.Empty;
            int one = sr.BaseStream.ReadByte();
            countSymbols++;
            String bufferEightSymbols = String.Empty;
            String restSymbols = String.Empty;
         
            while (one != -1)
            {
                int sym = one;
                if (weights.ContainsKey(sym))
                {
                    String value = restSymbols + weights[sym];
                    restSymbols = String.Empty;
                    while (value.Length > 7) {

                        getEightSymbols(ref value);
                    }
                    restSymbols += value;
                }
                //prBar.Maximum++;
                countSymbols++;
                one = sr.BaseStream.ReadByte();
            }
            int rest = (8 - restSymbols.Length);
            if (rest == 8)
            {
                sr.Close();
                return 0;
            }
            while (restSymbols.Length < 8)
            {
                countSymbols++;
                restSymbols += '0';
            }
            sr.Close();
            return rest;
        }


        public void readByteByByte(ref String m_fileName, ref int[] arr)
        {
            m_fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(m_fileStream);
            int one = sr.BaseStream.ReadByte();
            while (one != -1)
            {
                int sym = one;
                arr[sym]++;
                one = sr.BaseStream.ReadByte();
            }
            sr.Close();
            m_fileStream.Close();
        }

        private void WriteTreeInArchive(ref CHeap m_heap, ref  StreamWriter writerArchive)
        {
            int hSize = m_heap.heapSize;
            CHeap tmpHeap = new CHeap(m_heap);
            byte[] bytes = BitConverter.GetBytes(hSize);
            writerArchive.BaseStream.Write(bytes, 0, bytes.Length);
            for (int i = 0; i <= hSize; ++i)
            {
                CNode node = tmpHeap.nodes[i];
                writerArchive.BaseStream.WriteByte((byte)node.leaf.symbol);
                byte[] bts = BitConverter.GetBytes(node.leaf.frequency);
                writerArchive.BaseStream.Write(bts, 0, bts.Length);

            }
        }

        private void transform(ref String m_fileName, ref Dictionary<int, string> weights, ref StreamWriter writerArchive, int rest)
        {
            m_fileStream = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(m_fileStream);
            int one = sr.BaseStream.ReadByte();
            String restSymbols = String.Empty;
            while (one != -1)
            {
                int sym = one;
                if (weights.ContainsKey(sym))
                {
                    String value = restSymbols + weights[sym];
                    restSymbols = String.Empty;
                    while (value.Length > 7)
                    {

                        byte res = 0;
                        res = Convert.ToByte(getEightSymbols(ref value), 2);
                        writerArchive.BaseStream.WriteByte((byte)res);
                    }
                    restSymbols += value;
                }
                 one = sr.BaseStream.ReadByte();
            }
            if (rest > 0)
            {
                while (restSymbols.Length < 8)
                {
                    restSymbols += '0';
                }
                byte res = 0;
                res = Convert.ToByte(getEightSymbols(ref restSymbols), 2);
                writerArchive.BaseStream.WriteByte((byte)res);
            }
         
            sr.Close();
            m_fileStream.Close();
        }

    }
}
