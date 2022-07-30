using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impedio
{
    public class QuantumCircuit
    {
        public int QubitCount { get; private set; }
        //Declare simulator

        public QuantumCircuit(int qubitCount)
        {
            QubitCount = qubitCount;
            //Initalize simulator
        }
    }
}
