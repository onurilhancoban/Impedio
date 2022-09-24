using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Impedio;

namespace Impedio.Simulation
{
    public class Simulator
    {
        int qubitCount;
        List<QuantumGateContext> gateList;

        public Simulator(int _qubitCount)
        {
            if(_qubitCount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            qubitCount = _qubitCount;
            gateList = new List<QuantumGateContext>();
        }

        #region Gate Calculation

        public async Task<Matrix<Complex32>> CalculateCiruitMatrix()
        {
            GateSliceCalculator calculator = new GateSliceCalculator(qubitCount);

            //Calculate gate slices for all gates
            var gateSliceTasks = new List<Task>();
            
            foreach(var gateContext in gateList)
            {
                gateSliceTasks.Add(calculator.CalculateGateSliceAsync(gateContext));
            }
            await Task.WhenAll(gateSliceTasks);

            var gateSlices = new List<Matrix<Complex32>>();
            foreach(var task in gateSliceTasks)
            {
                gateSlices.Add(((Task<Matrix<Complex32>>)task).Result);
            }

            //Construct our final products matrix

            var gateDismensions = await Task.Run(() => gateSlices.FirstOrDefault().RowCount);
            var finalProduct = await Task.Run(() => Matrix<Complex32>.Build.SparseIdentity(gateDismensions));

            //qd
            gateSlices.Reverse();

            //Multiply all the gate slices together
            foreach (var gate in gateSlices)
            {
                await Task.Run(() => finalProduct = finalProduct * gate);
            }
            return finalProduct;
        }

        public async Task<Vector<Complex32>> CalculateStateVector(Matrix<Complex32> circuitMatrix)
        {
            //Initialize the state vector
            var svArray = Enumerable.Repeat(new Complex32(0, 0), circuitMatrix.RowCount).ToArray();
            svArray[0] = 1;
            var stateVector = await Task.Run(() => Vector<Complex32>.Build.SparseOfArray(svArray));

            //Multiply the sv with the circuit matrix and return the result
            stateVector = circuitMatrix * stateVector;
            return stateVector;
        }

        #endregion

        #region Gate List Operations

        public void AddGate(QuantumGateContext gate)
        {
            gateList.Add(gate);
        }

        public void ClearAllGates()
        {
            gateList.Clear();
        }

        #endregion
    }
}
