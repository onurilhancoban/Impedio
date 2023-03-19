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
        public static QuantumGate I = new QuantumGate(Matrix<Complex32>.Build.DenseIdentity(2));

        public static QuantumGate X = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
        {
            { 0, 1 },
            { 1, 0 }
        }));

        public static QuantumGate Y = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
        {
            { 0, new Complex32(0, -1) },
            { new Complex32(0, 1), 0 }
        }));

        public static QuantumGate Z = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
        {
            { 1, 0 },
            { 0, -1 }
        }));

        public static QuantumGate H = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
        {
            { new Complex32((float)Constants.Sqrt1Over2, 0), new Complex32((float)Constants.Sqrt1Over2, 0) },
            { new Complex32((float)Constants.Sqrt1Over2, 0), new Complex32(-(float)Constants.Sqrt1Over2, 0) }
        }));

        public static QuantumGate P(float phi)
        {
            var power = new Complex32(0, phi);
            var gate = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
            {
                { 1, 0 },
                { 0, Complex32.Exp(power) }
            }));
            return gate;
        }

        public static QuantumGate S = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
        {
            { 1, 0 },
            { 0, new Complex32(0, 1) }
        }));

        public static QuantumGate T = P((float)Constants.PiOver4);

        public static QuantumGate SWAP = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[4, 4]
        {
            { 1, 0 , 0 , 0},
            { 0, 0 , 1 , 0},
            { 0, 1 , 0 , 0},
            { 0, 0 , 0 , 1}
        }));

        public static QuantumGate Rxyz(float x, float y, float z)
        {
            Complex32 arg11 = Complex32.Cos(x/2);
            Complex32 arg12 = -1 * Complex32.Sin(x / 2) * Complex32.Exp(new Complex32(0, z));
            Complex32 arg21 = Complex32.Sin(x / 2) * Complex32.Exp(new Complex32(0, y));
            Complex32 arg22 = Complex32.Cos(x / 2) * Complex32.Exp(new Complex32(0, y + z));

            var gate = new QuantumGate(Matrix<Complex32>.Build.DenseOfArray(new Complex32[2, 2]
            {
                { arg11, arg12 },
                { arg21, arg22 }
            }));
            return gate;
        }
    }
}
