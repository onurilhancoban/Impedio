using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Simulator
{
    public class Simulator
    {
        int qubitCount;

        public Simulator(int _qubitCount)
        {
            if(_qubitCount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            qubitCount = _qubitCount;
        }


        //https://www.researchgate.net/publication/264003521_Efficient_algebraic_representation_of_quantum_circuits
        public Matrix<Complex32> CalculateGateSlice(QuantumGate gate, int qubitIndex)
        {
            if(qubitIndex < 1 || qubitIndex > qubitCount)
            {
                throw new ArgumentOutOfRangeException();
            }
            if(gate.ControlList.Any(s => s.Index > qubitCount))
            {
                throw new ArgumentOutOfRangeException();
            }
            if(gate.ControlList.Any(s => s.Index >= qubitIndex && s.Index < qubitIndex + gate.GateSize - 1))
            {
                throw new ArgumentOutOfRangeException();
            }

            var controlList = gate.ControlList.OrderBy(s => s.Index).ToList(); //Order controls from most significant (q0) to least (qN-1)
            bool[] controlInitArray = Enumerable.Repeat(false, controlList.Count).ToArray();

            var product = Matrix<Complex32>.Build.SparseIdentity(1); //Product of each term
            var sum = Matrix<Complex32>.Build.Sparse((int)Math.Pow(2, qubitCount), (int)Math.Pow(2, qubitCount)); //the final sum of all terms

            //Loop through N terms
            for (int i = 0; i < controlList.Count; i++)
            {
                //Loop through each qubit
                for(int j = 0; j < qubitCount; j++)
                {
                    //Check if there is a control on this qubit
                    if(controlList.Any(s => s.Index == j))
                    {
                        //Basically we perform an XOR on the init state and the type of the control to find out whether to use |0><0| or |1><1|
                        var theD = Convert.ToInt32(controlInitArray[j] != controlList[j].Type);
                        product = product.KroneckerProduct(Helpers.D[theD]);

                        //Initialize control if it isn't initialized already
                        if (controlInitArray[j] == false)
                        {
                            controlInitArray[j] = true;
                        }
                    }
                    //If there aren't any controls, multiply by identity.
                    else
                    {
                        product = product.KroneckerProduct(Helpers.Identity);
                    }
                }
                //Add the product to the sum,
                sum += product;
                //And reset the product.
                product = Matrix<Complex32>.Build.SparseIdentity(1);
            }
            //Loop through each qubit for the last term
            for(int k = 0; k < qubitCount; k++)
            {
                //If there is our gate on the qubit, apply the gate
                if(qubitIndex == k)
                {
                    product = product.KroneckerProduct(gate.GateMatrix);

                    //This will prevent the gate from being applied multiple times
                    k += (gate.GateSize - 1);
                }
                //Check if there is a control
                else if(controlList.Any(s => s.Index == k))
                {
                    //Apply the correct D depending on the control type.
                    var theD = Convert.ToInt32(!controlList[k].Type);
                    product = product.KroneckerProduct(Helpers.D[theD]);
                }
                else
                {
                    product = product.KroneckerProduct(Helpers.Identity);
                }
            }
            sum += product;

            return sum;
        }
    }
}
