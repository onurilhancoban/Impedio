using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Impedio.Simulation;

namespace Impedio
{
    public partial class QuantumGate
    {
        public Matrix<Complex32> GateMatrix { get; }
        public List<QuantumGateControl> ControlList { get; }

        //This is the qubit count of the base matrix without controls
        public int GateSize { get
            {
                return Math.ILogB(GateMatrix.RowCount);
            } }

        public bool HasControls { 
            get
            {
                return ControlList.Count != 0;
            }
        }

        private QuantumGate(Matrix<Complex32> matrix)
        {
            //Check if the matrix is a square matrix
            if(!(matrix.RowCount == matrix.ColumnCount))
            {
                throw new ArgumentException("The gate matrix must be a square matrix.");
            }

            GateMatrix = matrix;
            ControlList = new List<QuantumGateControl>();
        }

        public QuantumGate(QuantumGate gate) //Required for deep copy
        {
            GateMatrix = gate.GateMatrix;
            ControlList = gate.ControlList.ConvertAll(s => new QuantumGateControl(s.Type, s.Index)); //Deep copy list
        }

        public QuantumGate WithControl(int controlIndex, bool controlType = false)
        {
            if(controlIndex < 0 )
            {
                throw new ArgumentOutOfRangeException();
            }

            if(ControlList.Any(s => s.Index == controlIndex))
            {
                throw new ArgumentOutOfRangeException("There is already a control on that index!");
            }

            var newGate = new QuantumGate(this);
            newGate.ControlList.Add(new QuantumGateControl(controlType, controlIndex));
            return newGate;
        }
    }
}
