using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace attemptSecondHuffman
{
    public class CSequintialDeCompressor : IDecompressor
    {
        public void readHeap(ref CHeap m_heap, ref  StreamReader readerArchive)
        {
            int hSize = 0;
            int one = int.MaxValue;
            byte[] bts = BitConverter.GetBytes(one);
            if ((one = readerArchive.BaseStream.Read(bts, 0, bts.Length)) != -1)
            {
                hSize = BitConverter.ToInt32(bts, 0);
            }
            else
            {
                throw new Exception("Broken Archive");
            }
            for (int i = 0; i <= hSize; ++i)
            {

                int symbol;
                int frequency;
                int symByte;
                if ((symByte = readerArchive.BaseStream.ReadByte()) != -1)
                {
                    symbol = symByte;
                }
                else {
                    throw new Exception("Broken Archive");
                }
                if ((one = readerArchive.BaseStream.Read(bts, 0, bts.Length)) != -1)
                {
                    frequency = BitConverter.ToInt32(bts, 0);
                }
                else
                {
                    throw new Exception("Broken Archive");
                }
                m_heap.nodes[i].leaf.frequency = frequency;
                m_heap.nodes[i].leaf.symbol = symbol;
                m_heap.heapSize++;
            }

        }
        private int getRestSymbols(ref StreamReader readerArchive, List<String> arr, ref int countSymbols)
        {
            int one = readerArchive.BaseStream.ReadByte();
            int rest = one;
            int one2 = int.MaxValue;
            byte[] bts = BitConverter.GetBytes(one2);
            if ((one = readerArchive.BaseStream.Read(bts, 0, bts.Length)) != -1)
            {
                countSymbols = BitConverter.ToInt32(bts, 0);
            }
            else
            {
                throw new Exception("Broken Archive");
            }
            return rest;
        }

        public void transformArchiveToFile(ref CNode rootTree, String outFileName, ref  StreamReader readerArchive)
        {
            FileStream file1 = new FileStream("test1.txt", FileMode.Create); //создаем файловый поток
            StreamWriter writer = new StreamWriter(file1);
            FileStream fileOut = new FileStream(outFileName, FileMode.Create); //создаем файловый поток
            StreamWriter writerArchive = new StreamWriter(fileOut);
            String lines = string.Empty;
            CNode currentNode = rootTree;
            List<String> arr = new List<string>();
            int counSymbols = 0;
            int rest = getRestSymbols(ref readerArchive, arr, ref counSymbols);
            int one = readerArchive.BaseStream.ReadByte();
            int cnt = 1;
            while (one != -1)
            {
                String res = Convert.ToString(one, 2);
                Helper.appendsZero(ref res);
                int len;
                if (cnt != counSymbols || rest == 0)
                {
                    len = res.Count();
                }
                else
                {
                    len = res.Count()  - rest;
                }
                for (int j = 0; j < len; ++j)
                {
                    int sym = res[j];
                    if (currentNode.left == null && currentNode.right == null)
                    {
                        byte value = (byte)currentNode.leaf.symbol;
                        currentNode = rootTree;
                        writerArchive.BaseStream.WriteByte(value);
                    }
                    if (sym == '0')
                    {
                        currentNode = currentNode.left;
                    }
                    else
                    {
                        currentNode = currentNode.right;
                    }
                }
                one = readerArchive.BaseStream.ReadByte();
                cnt++;
            }

            for (int i = 0; i < arr.Count(); ++i)
            {
                int len = 0;
                if (i != (arr.Count() - 1) || rest == 0)
                {
                    len = arr[i].Count();
                }
                else
                {
                    len = arr[i].Count() - rest;
                }
                for (int j = 0; j < len; ++j)
                {
                    int sym = arr[i][j];
                    if (currentNode.left == null && currentNode.right == null)
                    {
                        byte value = (byte)currentNode.leaf.symbol;
                        currentNode = rootTree;
                        writerArchive.BaseStream.WriteByte(value);
                    }
                    if (sym == '0')
                    {
                        currentNode = currentNode.left;
                    }
                    else
                    {
                        currentNode = currentNode.right;
                    }
                }
            }
            if (currentNode.left == null && currentNode.right == null)
            {
                byte value = (byte)currentNode.leaf.symbol;
                currentNode = rootTree;
                writerArchive.BaseStream.WriteByte(value);
            }
            writerArchive.Close();
            fileOut.Close();
        }
    }
}
