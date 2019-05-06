using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZPI_Paletyzator.Model
{
    class OptimizationMain
    {
        public double packageHeight;
        public double packageWidth;
        public double packageLength;
        public double packageWeight;
        public bool seamAtBegin;

        public double palleteWidth;
        public double palleteLength;
        public double palleteMaxHeight;
        public double palleteMaxWeight;
       

        public double calculate()
        {
            if (seamAtBegin)
            {
                return packageHeight + packageWidth + packageLength + packageWeight + palleteLength + palleteWidth + palleteMaxHeight + palleteMaxWeight;
            }
            else
            {
                return (packageHeight + packageWidth + packageLength + packageWeight + palleteLength + palleteWidth + palleteMaxHeight + palleteMaxWeight) * -1;
            }
            
        }
    }
}
