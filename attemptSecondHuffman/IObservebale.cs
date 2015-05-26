using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemptSecondHuffman
{
    public interface IObservebale
    {
        void onStart(int finishValue);
        void onFinish();
        void onIncrement();
    }
}
