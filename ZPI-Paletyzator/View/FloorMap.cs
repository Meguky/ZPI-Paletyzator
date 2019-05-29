using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZPI_Paletyzator.Helper;

namespace ZPI_Paletyzator.View
{
    class FloorMap
    {
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }

        public List<MapNode> Map { get; private set; }

        //public int ObjectsPerFloor { get; private set; }

        public FloorMap (double packageWidth, double packageLength, double paletteWidth, double paletteLength)
        {
            Map = new List<MapNode>();
            PackageWidth = packageWidth;
            PackageLength = packageLength;
            PaletteWidth = paletteWidth;
            PaletteLength = paletteLength;
            GenerateMap();
        }

        private void GenerateMap ()
        {
            //MapNode node = new MapNode()
            //{
            //    PosX = -PaletteWidth,
            //    PosY = -PaletteLength
            //};
            //Map.Add(node);

            double nowX;
            nowX = -PaletteWidth;

            while (nowX + 2 * PackageWidth <= PaletteWidth)
            {
                double nowY = -PaletteLength;
                while (nowY + 2 * PackageLength <= PaletteLength)
                {
                    MapNode node = new MapNode()
                    {
                        PosX = nowX,
                        PosY = nowY,
                    };
                    Map.Add(node);
                    nowY += 2 * PackageLength;
                    nowY += 0.01;
                    //ObjectsPerFloor++;
                }
                nowX += 2 * PackageWidth;
                nowX += 0.01;
            }
        }
    }
}
