using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impedio.Simulation;

namespace Impedio
{
    public class QuantumCircuit
    {
        public int QubitCount { get; private set; }
        Simulator sex;

        public QuantumCircuit(int qubitCount)
        {
            QubitCount = qubitCount;
            //Initalize simulator
        }
    }
}
