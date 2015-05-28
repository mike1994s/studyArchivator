using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace attemptSecondHuffman
{
    public static class Helper
    {
        // Метод меняющий местами аргументы
       public static void SwapNode(ref CNode a, ref CNode b)
        {
            CNode c;
            c = a;
            a = b;
            b = c;
        }

       public static String getFileNameFromNameArchive(String archiveFileName)
        {
            string[] partsStrings = archiveFileName.Split('.');
            int size = partsStrings.Count();
            String outFileName = String.Empty;
            for (int j = 0; j < size - 1; ++j)
            {
                if (j != 0)
                {
                    outFileName += ".";
                }
                outFileName += partsStrings[j];
            }
            return outFileName;
        }

       public static void appendsZero(ref String str)
       {
           while (str.Length < 8)
           {
               str = '0' + str;
           }
       }

       public static String getAffix()
       {
           return "_heap_";
       }

       public static String getExtensionArchive()
       {
           return ".lema";
       }

       public static String getPostFixDirectory()
       {
           return "_LEMA";
       }

        public static void initHeap( ref CHeap heap)
        {
            for (int i = 0; i < heap.nodes.Count(); ++i)
            {
                heap.nodes[i] = new CNode();
                heap.nodes[i].leaf = new CLeaf();
            }
            heap.heapSize = -1;
        }

        public static void MultipleFilesToSingleFile(List<String> files, string destFile)
        {
            using (var destStream = File.Create(destFile))
            {
                foreach (string filePath in files)
                {
                    using (var sourceStream = File.OpenRead(filePath))
                        sourceStream.CopyTo(destStream); 
                    File.Delete(filePath);
                }
            }
        }
    }
}
