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

        public FloorMap (double packageWidth, double packageLength, double paletteWidth, double paletteLength, bool turned = false)
        {
            Map = new List<MapNode>();
            PackageWidth = packageWidth;
            PackageLength = packageLength;
            PaletteWidth = paletteWidth;
            PaletteLength = paletteLength;

            if (turned == false)
                GenerateMap();
            else
                GenerateTurnedMap();
        }

        private void GenerateMap ()
        {
            //MapNode node = new MapNode()
            //{
            //    PosX = -PaletteWidth,
            //    PosY = -PaletteLength
            //};
            //Map.Add(node);

            double nowX = -PaletteWidth;

            int columns = 0;
            while (nowX + 2 * PackageWidth <= PaletteWidth + 0.01 * columns)
            {
                int rows = 0;
                double nowY = -PaletteLength;
                while (nowY + 2 * PackageLength <= PaletteLength + 0.01 * rows)
                {
                    MapNode node = new MapNode()
                    {
                        PosX = nowX,
                        PosY = nowY,
                    };
                    Map.Add(node);
                    nowY += 2 * PackageLength;
                    nowY += 0.01;
                    rows++;
                }
                nowX += 2 * PackageWidth;
                nowX += 0.01;
                columns++;
            }
        }



        private void GenerateTurnedMap ()
        {
            double nowX = -PaletteWidth;

            int columns = 0;
            while (nowX + 2 * PackageLength <= PaletteWidth + 0.01 * columns)
            {
                int rows = 0;
                double nowY = -PaletteLength;
                while (nowY + 2 * PackageWidth <= PaletteLength + 0.01 * rows)
                {
                    MapNode node = new MapNode()
                    {
                        PosX = nowX,
                        PosY = nowY,
                        IsTurned = true,
                    };
                    Map.Add(node);
                    nowY += 2 * PackageWidth;
                    nowY += 0.01;
                    rows++;
                }
                nowX += 2 * PackageLength;
                nowX += 0.01;
                columns++;
            }
        }
    }
}
