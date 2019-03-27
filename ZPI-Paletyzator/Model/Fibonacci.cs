using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ZPI_Paletyzator.Model
{
    public class Fibonacci
    {
        public double FibNum;
        public double FibNumPrev;

        public Fibonacci()
        {
            FibNum = 1;
            FibNumPrev = 1;
        }

        public double calculateNext()
        {
            var next = FibNum + FibNumPrev;
            FibNumPrev = FibNum;
            FibNum = next;
            return next;
        }
    }
}
