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
using ZPI_Paletyzator.Helper;


namespace ZPI_Paletyzator.View
{
    class SceneObjectsGenerator
    {
        private double PackageHeight { get; set; }
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }
        private double Levels { get; set; }
        private double _visualMilimeter;

        public SceneObjectsGenerator(double packageHeight = 0, double packageWidth = 0, double packageLength = 0, double paletteWidth = 0, double paletteLength = 0, int levels = 0)
        {
            Levels = levels;
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
            var mainModel = new Model3DGroup();
            mainModel.Children.Add(FlatPartGenerator());
            mainModel.Children.Add(GroundPartsGenerator());
            mainModel.Children.Add(GroundGenerator());

            if (PackageHeight > 0 && PackageWidth > 0 && PackageLength > 0 && PaletteWidth > 0 && PaletteLength > 0)
            {
                FloorMap floorMap = new FloorMap(PackageWidth, PackageLength, PaletteWidth, PaletteLength);
                var packageFloors = new Model3DGroup();
                packageFloors.Children.Add(FloorGenerator(floorMap, PackageGenerator()));


                var coloredFloors = new Model3DGroup();
                coloredFloors = ThrowUpRainbow(packageFloors);
                mainModel.Children.Add(coloredFloors);
            }
            return mainModel;
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



        private GeometryModel3D PackageGenerator()
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

            MeshGeometry3D palletMesh = new MeshGeometry3D
            {
                Positions = meshPoints,
                TriangleIndices = triangleIndices
            };

            GeometryModel3D model = new GeometryModel3D
            {
                Geometry = palletMesh,            
            };

            model.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Magenta));

            return model;
        }



        private Model3DGroup FloorGenerator (FloorMap floorMap, GeometryModel3D model)
        {
            Model3DGroup floor = new Model3DGroup();
            foreach (MapNode mapNode in floorMap.Map)
            {
                GeometryModel3D geometryModel = new GeometryModel3D();
                geometryModel = model.Clone();
                TranslateTransform3D translation = new TranslateTransform3D(new Vector3D(mapNode.PosX, PackageHeight, - mapNode.PosY));
                RotateTransform3D rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90));
                Transform3DGroup transformation = new Transform3DGroup();

                if (mapNode.IsTurned)
                    transformation.Children.Add(rotation);

                transformation.Children.Add(translation);
                geometryModel.Transform = transformation;

                floor.Children.Add(geometryModel);
            }

            double min_X, max_X, min_Z, max_Z;
            min_X = min_Z = double.MaxValue;
            max_X = max_Z = double.MinValue;

            for (int i = 0; i < floor.Children.Count; i++)
            {
                GeometryModel3D geometryModel = new GeometryModel3D();
                geometryModel = (GeometryModel3D) floor.Children[i];

                Transform3DGroup translationGroup = (Transform3DGroup)geometryModel.Transform;
                TranslateTransform3D translation = (TranslateTransform3D) translationGroup.Children[translationGroup.Children.Count - 1];

                double minCandidateX, maxCandidateX, minCandidateZ, maxCandidateZ;

                if (translationGroup.Children.Count > 1)
                {
                    minCandidateX = translation.OffsetX - PackageLength;
                    maxCandidateX = translation.OffsetX + PackageLength;
                    minCandidateZ = translation.OffsetZ - PackageWidth;
                    maxCandidateZ = translation.OffsetZ + PackageWidth;
                }
                else
                {
                    minCandidateX = translation.OffsetX - PackageWidth;
                    maxCandidateX = translation.OffsetX + PackageWidth;
                    minCandidateZ = translation.OffsetZ - PackageLength;
                    maxCandidateZ = translation.OffsetZ + PackageLength;
                }
                
                if (min_X > minCandidateX)
                    min_X = minCandidateX;

                if (max_X < maxCandidateX)
                    max_X = maxCandidateX;

                if (min_Z > minCandidateZ)
                    min_Z = minCandidateZ;

                if (max_Z < maxCandidateZ)
                    max_Z = maxCandidateZ;

                Model3DGroup modelGroup = new Model3DGroup();
                modelGroup.Children.Add(geometryModel);
                floor.Children[i] = modelGroup;
            }

            floor.Transform = new TranslateTransform3D(new Vector3D( - (max_X - (max_X - min_X) / 2), 0, -(max_Z - (max_Z - min_Z) / 2)));
            return floor;
        }



        private Model3DGroup ThrowUpRainbow (Model3DGroup sadFloors)
        {
            int numberOfPackages = 0;
            for (int i = 0; i < sadFloors.Children.Count; i++)
            {
                Model3DGroup packages = new Model3DGroup();
                packages = (Model3DGroup) sadFloors.Children[i];
                numberOfPackages += packages.Children.Count;
            }

            ColourGenerator colourGenerator = new ColourGenerator(numberOfPackages);

            for (int i = 0; i < sadFloors.Children.Count; i++)
            {
                Model3DGroup packages = new Model3DGroup();
                packages = (Model3DGroup)sadFloors.Children[i];
                for (int j = 0; j < packages.Children.Count; j++)
                {
                    Model3DGroup onePackage = new Model3DGroup();
                    onePackage = (Model3DGroup) packages.Children[j];
                    GeometryModel3D model = new GeometryModel3D();
                    model = (GeometryModel3D) onePackage.Children[0];
                    DiffuseMaterial material = new DiffuseMaterial();
                    material = (DiffuseMaterial)model.Material;
                    material.Brush = new SolidColorBrush(colourGenerator.GetColor());
                    model.Material = material;
                    onePackage.Children[0] = model;
                    packages.Children[j] = onePackage;
                }
                sadFloors.Children[i] = packages;
            }



            return sadFloors;
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
                new Point3D(0.001, PackageHeight, -PackageLength),
                new Point3D(0.001, 0,             -PackageLength),
                new Point3D(0.001, 0,              PackageLength),
                new Point3D(0.001, PackageHeight,  PackageLength)
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