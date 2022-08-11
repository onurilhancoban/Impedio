using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Impedio.Simulation
{
    public class QuantumGate
    {
        public Matrix<Complex32> GateMatrix { get; }
        public List<QuantumGateControl> ControlList { get; }

        //Please note that this is the qubit count of the base matrix without controls
        public int GateSize { get
            {
                return Math.ILogB(GateMatrix.RowCount);
            } }

        public QuantumGate(Matrix<Complex32> matrix)
        {
            //Check if the matrix is a square matrix
            if(matrix.RowCount == matrix.ColumnCount)
            {
                throw new ArgumentException("The gate matrix must be a square matrix.");
            }

            //Check if the matrix is unitary
            if(matrix * matrix.ConjugateTranspose() == Matrix<Complex32>.Build.SparseIdentity(matrix.RowCount, matrix.ColumnCount))
            {
                throw new ArgumentException("The gate matrix must be unitary");
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
            if(ControlList.Any(s => s.Index == controlIndex))
            {
                throw new ArgumentOutOfRangeException("There is already a qubit on that index!");
            }

            var newGate = new QuantumGate(this);
            newGate.ControlList.Add(new QuantumGateControl(controlType, controlIndex));
            return newGate;
        }

        #region Default Gates

        public static Matrix<Complex32> Identity = Matrix<Complex32>.Build.DenseIdentity(2);

        #endregion
    }
}
