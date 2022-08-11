using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impedio.Simulation
{
    public class QuantumGateContext
    {
        public QuantumGate Gate { get; }
        public int Index { get; }

        public QuantumGateContext(QuantumGate gate, int index)
        {
            Gate = gate;
            Index = index;
        }
    }
}
