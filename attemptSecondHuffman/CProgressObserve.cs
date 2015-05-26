using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace attemptSecondHuffman
{
   public class CProgressObserve : IObservebale
    {

       private int m_startValue;
       private int m_currentValue;
       private int m_finishValue;
       private ProgressBar m_progressBar;
       public CProgressObserve(ref ProgressBar prBar)
       {
           m_progressBar = prBar;
           m_progressBar.Visible = true;
           Application.DoEvents();
       }
       public void onStart(int finishValue)
       {
           m_startValue = 0;
           m_currentValue = 0;
           m_finishValue = finishValue;
           m_progressBar.Maximum = m_finishValue;
       }
       public void onFinish()
       {
           m_progressBar.Value = m_finishValue;
       }
       public void onIncrement()
       {
           m_progressBar.Value++;
       }
    }
}
