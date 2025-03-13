using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockRunner
{
    public interface IObserver
    {
        void Update(int timeElapsed);
    }
}
