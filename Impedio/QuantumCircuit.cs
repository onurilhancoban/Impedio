using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impedio.Simulation;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Impedio
{
    public class QuantumCircuit
    {
        public int QubitCount { get; private set; }
        Simulator simulator;

        public QuantumCircuit(int qubitCount)
        {
            if(qubitCount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            QubitCount = qubitCount;
            simulator = new Simulator(qubitCount);
        }
        
        public void ApplyGate(QuantumGate gate, int index)
        {
            if(index < 0 || index > QubitCount)
            {
                throw new ArgumentOutOfRangeException();
            }

            var gateContext = new QuantumGateContext(gate, index);
            simulator.AddGate(gateContext);
        }
        
        public async Task<Vector<Complex32>> SV_CALCULATE_DEBUG()
        {
            var cm = await simulator.CalculateCiruitMatrix();
            return await simulator.CalculateStateVector(cm);
        }
    }
}
