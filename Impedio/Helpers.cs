using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impedio
{
    public static class Helpers
    {
        public static string GetStateString(this QuantumCircuit circuit, int index)
        {
            return Convert.ToString(index, 2).PadLeft(circuit.QubitCount, '0');
        }

        public static int GetStateIndex(this QuantumCircuit circuit, string binary)
        {
            return Convert.ToInt32(binary, 2);
        }
    }
}
