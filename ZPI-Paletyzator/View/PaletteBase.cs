using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ZPI_Paletyzator.View
{
    class PaletteBase
    {
        private double PackageHeight { get; set; }
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }
        private double _visualMilimeter;


        public PaletteBase(double packageHeight = 0, double packageWidth = 0, double packageLength = 0, double paletteWidth = 0, double paletteLength = 0)
        {
            PaletteLength = 4.5;
            double palLeng = 0;
            if (paletteWidth > paletteLength)
            {
                PaletteWidth = paletteLength;
                palLeng = paletteWidth;
            }
            else
            {
                PaletteWidth = paletteWidth;
                palLeng = paletteLength;
            }

            if (palLeng > 0)
            {
                _visualMilimeter = PaletteLength / palLeng;
                PaletteWidth *= _visualMilimeter;
                PackageHeight = packageHeight * _visualMilimeter;
                PackageWidth = packageWidth * _visualMilimeter;
                PackageLength = packageLength * _visualMilimeter;
            }
            else
                PaletteWidth = 3;
        }

        public Model3DGroup GetModel()
        {
            var model = new Model3DGroup();
            model.Children.Add(FlatPartGenerator());
            model.Children.Add(GroundPartsGenerator());
            model.Children.Add(GroundGenerator());

            return model;
        }

        private GeometryModel3D FlatPartGenerator()
        {
            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(- PaletteWidth,  0, - PaletteLength),
                new Point3D(- PaletteWidth,  0, PaletteLength),
                new Point3D(PaletteWidth,    0, PaletteLength),
                new Point3D(PaletteWidth,    0, - PaletteLength),

                new Point3D(- PaletteWidth, -0.4, - PaletteLength),
                new Point3D(- PaletteWidth, -0.4, PaletteLength),
                new Point3D(PaletteWidth,   -0.4, PaletteLength),
                new Point3D(PaletteWidth,   -0.4, - PaletteLength)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,1,2,
                2,3,0,
                0,4,5,
                5,1,0,
                1,5,6,
                6,2,1,
                2,6,3,
                3,6,7,
                3,7,4,
                4,0,3,
                4,7,6,
                6,5,4
            };

            MeshGeometry3D palletFlatMesh = new MeshGeometry3D
            {
                Positions = meshPoints,
                TriangleIndices = triangleIndices
            };

            GeometryModel3D model = new GeometryModel3D
            {
                Geometry = palletFlatMesh,
            };

            model.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Tan));

            return model;
        }



        private Model3DGroup GroundPartsGenerator()
        {

            var groundGroup = new Model3DGroup();

            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(-PaletteWidth,     -0.4, - PaletteLength),
                new Point3D(-PaletteWidth,     -0.4, PaletteLength),
                new Point3D(-PaletteWidth + 1, -0.4, PaletteLength),
                new Point3D(-PaletteWidth + 1, -0.4, - PaletteLength),

                new Point3D(-PaletteWidth,     -0.8, - PaletteLength),
                new Point3D(-PaletteWidth,     -0.8, PaletteLength),
                new Point3D(-PaletteWidth + 1, -0.8, PaletteLength),
                new Point3D(-PaletteWidth + 1, -0.8, - PaletteLength)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,4,5,
                5,1,0,
                1,5,6,
                6,2,1,
                2,6,7,
                7,3,2,
                3,7,4,
                4,0,3,
                5,4,7,
                7,6,5
            };

            MeshGeometry3D mesh = new MeshGeometry3D
            {
                Positions = meshPoints,
                TriangleIndices = triangleIndices
            };

            GeometryModel3D model = new GeometryModel3D
            {
                Geometry = mesh
            };

            model.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Bisque));


            groundGroup.Children.Add(model);
            groundGroup.Children.Add(model.Clone());
            groundGroup.Children.Add(model.Clone());


            groundGroup.Children[1].Transform = new TranslateTransform3D(PaletteWidth - 0.5, 0, 0);
            groundGroup.Children[2].Transform = new TranslateTransform3D(2 * PaletteWidth - 1, 0, 0);

            return groundGroup;
        }



        private GeometryModel3D GroundGenerator()
        {
            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(-30, -0.8, -30),
                new Point3D(-30, -0.8, 30),
                new Point3D(30, -0.8, 30),
                new Point3D(30, -0.8, -30)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,1,2,
                2,3,0
            };


            MeshGeometry3D mesh = new MeshGeometry3D
            {
                Positions = meshPoints,
                TriangleIndices = triangleIndices
            };

            GeometryModel3D model = new GeometryModel3D
            {
                Geometry = mesh
            };

            model.Material = new DiffuseMaterial(new SolidColorBrush(Colors.YellowGreen));

            return model;
        }
    }
}
