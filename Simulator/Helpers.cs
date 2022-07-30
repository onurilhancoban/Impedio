using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Simulator
{
    public class Helpers
    {
        //Used when calculating gate slices
        public static Matrix<Complex32> D0 = Matrix<Complex32>.Build.DenseOfArray(new Complex32[,] {
            { 1, 0 }, 
            { 0, 0 } 
        }); // |0><0|
        public static Matrix<Complex32> D1 = Matrix<Complex32>.Build.DenseOfArray(new Complex32[,] {
            { 0, 0 },
            { 0, 1 }
        }); // |1><1|
        public static Matrix<Complex32>[] D = { D0, D1 };

        //Never not useful
        public static Matrix<Complex32> Identity = Matrix<Complex32>.Build.DenseIdentity(2);
    }
}
