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
        public bool seamFacingFront;

        public double paletteWidth;
        public double paletteLength;
        public double paletteMaxHeight;
        public double paletteMaxWeight;
       

        public double Calculate()
        {
            if (seamFacingFront)
            {
                return packageHeight + packageWidth + packageLength + packageWeight + paletteLength + paletteWidth + paletteMaxHeight + paletteMaxWeight;
            }
            else
            {
                return (packageHeight + packageWidth + packageLength + packageWeight + paletteLength + paletteWidth + paletteMaxHeight + paletteMaxWeight) * -1;
            }
            
        }
    }
}
