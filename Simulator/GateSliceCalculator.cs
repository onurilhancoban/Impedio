using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impedio.Simulation;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Impedio.Simulation
{
    //https://www.researchgate.net/publication/264003521_Efficient_algebraic_representation_of_quantum_circuits
    public class GateSliceCalculator
    {
        int qubitCount;

        public GateSliceCalculator(int qubitCount)
        {
            this.qubitCount = qubitCount;
        }

        //Used when calculating gate slices
        public static Matrix<Complex32> D0 = Matrix<Complex32>.Build.DenseOfArray(new Complex32[,] // |0><0|
        {
            { 1, 0 },
            { 0, 0 }
        }); 
        public static Matrix<Complex32> D1 = Matrix<Complex32>.Build.DenseOfArray(new Complex32[,] // |1><1| 
        {
            { 0, 0 },
            { 0, 1 }
        }); 

        //Never not useful
        public static Matrix<Complex32> Identity = Matrix<Complex32>.Build.DenseIdentity(2);

        public static async Task<Matrix<Complex32>> EndlessPainAndSuffering(int controlIndex, int totalTerms, int selectedTerm, bool controlType)
        {
            var I = Identity;
            var deeZero = controlType ? D1 : D0;
            var deeOne = controlType ? D0 : D1;

            var tableArray = Enumerable.Repeat(deeOne, totalTerms).ToArray();

            await Task.Run(() =>
            {
                for (int i = 0; i < totalTerms; i++)
                {
                    if (i == controlIndex)
                    {
                        tableArray[i] = deeZero;
                    }
                    else if (i < controlIndex)
                    {
                        tableArray[i] = I;
                    }
                }
            });

            return tableArray[selectedTerm];
        }

        private async Task<Matrix<Complex32>> CalculateNonControlledGateAsync(QuantumGateContext context)
        {
            var product = await Task.Run(() => Matrix<Complex32>.Build.SparseIdentity(1));

            if (context.Gate.HasControls)
            {
                throw new InvalidOperationException();
            }

            for(var i = 0; i < qubitCount; i++)
            {
                if (context.Index == i)
                {
                    product = await Task.Run(() => product.KroneckerProduct(context.Gate.GateMatrix));

                    //This will prevent the gate from being applied multiple times
                    i += (context.Gate.GateSize - 1);
                }
                else
                {
                    product = await Task.Run(() => product.KroneckerProduct(Identity));
                }
            }

            return product;
        }

        private async Task<Matrix<Complex32>> CalculateControlledGateAsync(QuantumGateContext context)
        {
            if(!context.Gate.HasControls)
            {
                throw new InvalidOperationException();
            }

            var controlList = context.Gate.ControlList.OrderBy(s => s.Index).ToList(); //Order controls from most significant (q0) to least (qN-1)

            foreach(var control in controlList)
            {
                Console.WriteLine(control.Index);
            }

            var product = await Task.Run(() => Matrix<Complex32>.Build.SparseIdentity(1)); //Product of each term
            var sum = await Task.Run(() => Matrix<Complex32>.Build.Sparse((int)Math.Pow(2, qubitCount), (int)Math.Pow(2, qubitCount))); //the final sum of all terms

            //Control terms
            for(int i = 0; i < controlList.Count; i++)
            {
                for(int j = 0; j < qubitCount; j++)
                {
                    if(controlList.Any(s => s.Index == j))
                    {
                        var controlIndex = await Task.Run(() => controlList.FindIndex(s => s.Index == j));
                        var ligmoid = await EndlessPainAndSuffering(controlIndex, controlList.Count, i, controlList[controlIndex].Type);
                        product = await Task.Run(() => product.KroneckerProduct(ligmoid));
                    }
                    else
                    {
                        product = await Task.Run(() => product.KroneckerProduct(Identity));
                    }
                }
                //Add the product to the sum,
                sum = await Task.Run(() => sum + product);
                //And reset the product.
                product = await Task.Run(() => Matrix<Complex32>.Build.SparseIdentity(1));
            }
            //Last term
            for(int k = 0; k < qubitCount; k++)
            {
                if(controlList.Any(s => s.Index == k))
                {
                    var theD = controlList.Where(s => s.Index == k).FirstOrDefault().Type ? D0 : D1;
                    product = await Task.Run(() => product.KroneckerProduct(theD));
                }
                else if (context.Index == k)
                {
                    product = await Task.Run(() => product.KroneckerProduct(context.Gate.GateMatrix));

                    //This will prevent the gate from being applied multiple times
                    k += (context.Gate.GateSize - 1);
                }
                else
                {
                    product = await Task.Run(() => product.KroneckerProduct(Identity));
                }
            }
            //Add the product to the sum,
            sum = await Task.Run(() => sum + product);
            Console.WriteLine();

            return sum;
        }

        public async Task<Matrix<Complex32>> CalculateGateSliceAsync(QuantumGateContext context)
        { 
            if(context.Gate.HasControls)
            {
                return await CalculateControlledGateAsync(context);
            }
            else
            {
                return await CalculateNonControlledGateAsync(context);
            }
        }
    }
}
