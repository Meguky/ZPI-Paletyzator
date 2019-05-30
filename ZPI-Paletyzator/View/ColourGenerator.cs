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

        private int state;
        private double delta;
        private bool goUp = true;

        public ColourGenerator(int objectAmount)
        {
            if (objectAmount > 2)
            {
                state = -1;
                delta = 7.0 * 255 / (objectAmount - 1);
            }
            else
            {
                delta = 0;
            }
        }


        public Color GetColor ()
        {
            double toAdd = delta;

            if (delta == 0)
            {
                switch (state)
                {
                    case 0:
                        state++;
                        return Color.FromRgb(0, 0, 0);
                    case 1:
                        state--;
                        return Color.FromRgb(255, 255, 255);
                }
            }
            else
            {
                while (toAdd > 0)
                {
                    switch (state)
                    {
                        case -1:
                            goUp = true;
                            state++;
                            return Color.FromRgb(0, 0, 0);
                        case 0:
                            if (goUp)
                                Increase(ref R, ref toAdd);
                            else
                                Decrese(ref G, ref toAdd);
                            break;
                        case 1:
                            if (goUp)
                                Increase(ref G, ref toAdd);
                            else
                                Decrese(ref R, ref toAdd);
                            break;
                        case 2:
                            if (goUp)
                                Decrese(ref R, ref toAdd);
                            else
                                Increase(ref G, ref toAdd);
                            break;
                        case 3:
                            if (goUp)
                                Increase(ref B, ref toAdd);
                            else
                                Decrese(ref B, ref toAdd);
                            break;
                        case 4:
                            if (goUp)
                                Decrese(ref G, ref toAdd);
                            else
                                Increase(ref R, ref toAdd);
                            break;
                        case 5:
                            if (goUp)
                                Increase(ref R, ref toAdd);
                            else
                                Decrese(ref G, ref toAdd);
                            break;
                        case 6:
                            if (goUp)
                                Increase(ref G, ref toAdd);
                            else
                                Decrese(ref R, ref toAdd);
                            break;
                        case 7:
                            goUp = false;
                            state--;
                            return Color.FromRgb(255, 255, 255);

                        default:
                            return Color.FromRgb(0, 0, 0);
                    }
                }
            }
            return Color.FromRgb((byte)R, (byte)G, (byte)B);
        }



        private void Increase (ref double rgb, ref double toAdd)
        {
            double filler = 255 - rgb;
            if (toAdd >= filler)
            {
                rgb = 255;
                toAdd -= filler;
                if (goUp)
                    state++;
                else
                    state--;
            }
            else
            {
                rgb += toAdd;
                toAdd = 0;
            }
        }



        private void Decrese (ref double rgb, ref double toAdd)
        {
            if (toAdd >= rgb)
            {
                toAdd -= rgb;
                rgb = 0;
                if (goUp)
                    state++;
                else
                    state--;
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
            goUp = true;
        }
    }
}
