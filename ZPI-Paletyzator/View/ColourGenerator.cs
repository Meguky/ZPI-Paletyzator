using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZPI_Paletyzator.View
{
    class ColourGenerator
    {
        private double R;
        private double G;
        private double B;

        private byte state;

        private double delta;

        public ColourGenerator(int objectAmount)
        {
            if (objectAmount > 0)
            {
                delta = 7 * 255 / objectAmount;
            }
        }


        public Color GetColor ()
        {
            double toAdd = delta;

            while (toAdd > 0)
            {
                switch (state)
                {
                    case 0:
                        Increase(ref R, ref toAdd);
                        break;
                    case 1:
                        Increase(ref G, ref toAdd);
                        break;
                    case 2:
                        Decrese(ref R, ref toAdd);
                        break;
                    case 3:
                        Increase(ref B, ref toAdd);
                        break;
                    case 4:
                        Decrese(ref G, ref toAdd);
                        break;
                    case 5:
                        Increase(ref R, ref toAdd);
                        break;
                    case 6:
                        Increase(ref G, ref toAdd);
                        break;
                    default:
                        return Color.FromRgb(255, 255, 255);
                }
            }


            return Color.FromRgb((byte)R, (byte)G, (byte)B);
        }



        private void Increase (ref double rgb, ref double toAdd)
        {
            double filler = 255 - rgb;
            if (toAdd > filler)
            {
                rgb = 255;
                toAdd -= filler;
                state++;
            }
            else
            {
                rgb += toAdd;
                toAdd = 0;
            }
        }



        private void Decrese (ref double rgb, ref double toAdd)
        {
            if (toAdd > rgb)
            {
                toAdd -= rgb;
                rgb = 0;
                state++;
            }
            else
            {
                rgb -= toAdd;
                toAdd = 0;
            }
        }



        public void Reset ()
        {
            R = G = B = delta = state = 0;
        }
    }
}
