using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;


namespace ZPI_Paletyzator.View
{
    class SceneObjectsGenerator
    {
        private double PackageHeight { get; set; }
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }
        private const double _visualPaletteLength = 4.5;
        private double _visualPaleteWidth;
        private double _visualMilimeter;

        public SceneObjectsGenerator(double packageHeight = 0, double packageWidth = 0, double packageLength = 0, double paletteWidth = 0, double paletteLength = 0)
        {
            if (paletteWidth > paletteLength)
            {
                PaletteWidth = paletteLength;
                PaletteLength = paletteWidth;
            }
            else
            {
                PaletteWidth = paletteWidth;
                PaletteLength = paletteLength;
            }

            if (PaletteLength > 0)
            {
                _visualMilimeter = _visualPaletteLength / PaletteLength;
                _visualPaleteWidth = PaletteWidth * _visualMilimeter;
                PackageHeight = packageHeight * _visualMilimeter;
                PackageWidth = packageWidth * _visualMilimeter;
                PackageLength = packageLength * _visualMilimeter;
            }
            else
                _visualPaleteWidth = 3;
        }



        public Model3DGroup GetModel()
        {
            var mainModel = new Model3DGroup();
            mainModel.Children.Add(FlatPartGenerator());
            mainModel.Children.Add(GroundPartsGenerator());
            mainModel.Children.Add(GroundGenerator());

            if (PackageHeight > 0 && PackageWidth > 0 && PackageLength > 0 && PaletteWidth > 0 && PaletteLength > 0)
            {
                mainModel.Children.Add(PackagesGenerator());
                //mainModel.Children.Add(SignGenerator());
            }
            return mainModel;
        }




        private GeometryModel3D FlatPartGenerator()
        {
            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(- _visualPaleteWidth,  0, - _visualPaletteLength),
                new Point3D(- _visualPaleteWidth,  0, _visualPaletteLength),
                new Point3D(_visualPaleteWidth,    0, _visualPaletteLength),
                new Point3D(_visualPaleteWidth,    0, - _visualPaletteLength),

                new Point3D(- _visualPaleteWidth, -0.4, - _visualPaletteLength),
                new Point3D(- _visualPaleteWidth, -0.4, _visualPaletteLength),
                new Point3D(_visualPaleteWidth,   -0.4, _visualPaletteLength),
                new Point3D(_visualPaleteWidth,   -0.4, - _visualPaletteLength)
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
                new Point3D(-_visualPaleteWidth,     -0.4, - _visualPaletteLength),
                new Point3D(-_visualPaleteWidth,     -0.4, _visualPaletteLength),
                new Point3D(-_visualPaleteWidth + 1, -0.4, _visualPaletteLength),
                new Point3D(-_visualPaleteWidth + 1, -0.4, - _visualPaletteLength),

                new Point3D(-_visualPaleteWidth,     -0.8, - _visualPaletteLength),
                new Point3D(-_visualPaleteWidth,     -0.8, _visualPaletteLength),
                new Point3D(-_visualPaleteWidth + 1, -0.8, _visualPaletteLength),
                new Point3D(-_visualPaleteWidth + 1, -0.8, - _visualPaletteLength)
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


            groundGroup.Children[1].Transform = new TranslateTransform3D( _visualPaleteWidth - 0.5, 0, 0);
            groundGroup.Children[2].Transform = new TranslateTransform3D(2 * _visualPaleteWidth - 1, 0, 0);

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


        private Model3DGroup PackagesGenerator()
        {
            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(- PackageWidth,  PackageHeight, - PackageLength),
                new Point3D(- PackageWidth,  PackageHeight, PackageLength),
                new Point3D(PackageWidth,    PackageHeight, PackageLength),
                new Point3D(PackageWidth,    PackageHeight, - PackageLength),

                new Point3D(- PackageWidth, -PackageHeight, - PackageLength),
                new Point3D(- PackageWidth, -PackageHeight, PackageLength),
                new Point3D(PackageWidth,   -PackageHeight, PackageLength),
                new Point3D(PackageWidth,   -PackageHeight, - PackageLength)
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

            model.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Magenta));

            var modelGroup = new Model3DGroup();
            modelGroup.Children.Add(model);
            modelGroup.Transform = new TranslateTransform3D(new Vector3D(0, PackageHeight, 0));

            return modelGroup;
        }


        private Model3DGroup SignGenerator()
        {
            string text = "Noooo siema...";
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };

            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = new VisualBrush(textBlock)
            };

            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(0.01, PackageHeight, -PackageLength),
                new Point3D(0.01, 0,             -PackageLength),
                new Point3D(0.01, 0,              PackageLength),
                new Point3D(0.01, PackageHeight,  PackageLength)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,3,2,
                2,1,0
                //0,1,2,
                //2,3,0
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),
                new Point(0, 0)
            };

            MeshGeometry3D textMesh = new MeshGeometry3D
            {
                Positions = textPointCollection,
                TriangleIndices = triangleIndices,
                TextureCoordinates = textureCoordinates
            };

            GeometryModel3D textModel = new GeometryModel3D
            {
                Geometry = textMesh,
                Material = material
            };

            //GeometryModel3D backTextModel = textModel.Clone();
            //backTextModel.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180));

            var group = new Model3DGroup();
            group.Children.Add(textModel);
            //group.Children.Add(backTextModel);

            group.Transform = new TranslateTransform3D(new Vector3D(PackageWidth, 0, 0));

            return group;
        }
    }
}