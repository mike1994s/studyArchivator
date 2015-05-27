using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace attemptSecondHuffman
{
    class CParallelWorkflow : AWorkFlow
    {
        public CParallelWorkflow(AHeapPriority heapPriority, string fileName, AWorkFlow.Scenario scenario)
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
        protected override void compress() { }

        protected override void deCompress() { }
    }
   

}
