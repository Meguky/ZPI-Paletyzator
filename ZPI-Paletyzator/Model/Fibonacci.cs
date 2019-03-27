using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://intellitect.com/getting-started-model-view-viewmodel-mvvm-pattern-using-windows-presentation-framework-wpf/
/// </summary>


namespace ZPI_Paletyzator.Model
{
    public class Fibonacci
    {
        public double fibNum;
        public double fibNumPrev;

        public Fibonacci()
        {
            fibNum = 1;
            fibNumPrev = 1;
        }

        public void calculateNext()
        {
            var next = fibNum + fibNumPrev;
            fibNumPrev = fibNum;
            fibNum = next;
        }
    }
}
