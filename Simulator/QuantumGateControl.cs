using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class QuantumGateControl
    {
        public bool Type { get; }
        public int Index { get; }

        public QuantumGateControl(bool type, int index)
        {
            if(index < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Type = type;
            Index = index;
        }
    }
}
