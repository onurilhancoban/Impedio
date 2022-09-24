using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impedio.Simulation
{
    public class QuantumGateControl
    {
        public bool Type { get; } //Positive if false, Negative if true
        public int Index { get; }

        public QuantumGateControl(bool type, int index)
        {
            if(index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Type = type;
            Index = index;
        }
    }
}
