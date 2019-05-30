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
    class PackagesGenerator
    {
        private double PackageHeight { get; set; }
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }
        private double _visualMilimeter;
        private double Levels { get; set; }

        public PackagesGenerator(double packageHeight = 0, double packageWidth = 0, double packageLength = 0, double paletteWidth = 0, double paletteLength = 0, int levels = 0)
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

            if (PackageHeight > 0 && PackageWidth > 0 && PackageLength > 0 && PaletteWidth > 0 && PaletteLength > 0)
            {
                FloorMap floorMap = new FloorMap(PackageWidth, PackageLength, PaletteWidth, PaletteLength);
                FloorMap floorTurnedMap = new FloorMap(PackageWidth, PackageLength, PaletteWidth, PaletteLength, true);
                var packageFloors = new Model3DGroup();

                for (int i = 0; i < Levels; i++)
                {
                    packageFloors.Children.Add(FloorGenerator(floorMap, PackageGenerator(), i));
                    TranslateTransform3D translate = new TranslateTransform3D();
                    translate = (TranslateTransform3D)packageFloors.Children[i].Transform;
                    packageFloors.Children[i].Transform = new TranslateTransform3D(new Vector3D(translate.OffsetX, translate.OffsetY + PackageHeight * 2 * i + i * 0.01, translate.OffsetZ));

                    if (++i >= Levels)
                        break;
                    packageFloors.Children.Add(FloorGenerator(floorTurnedMap, PackageGenerator(), i));
                    TranslateTransform3D turnedFloorTranslate = new TranslateTransform3D();
                    turnedFloorTranslate = (TranslateTransform3D)packageFloors.Children[i].Transform;
                    packageFloors.Children[i].Transform = new TranslateTransform3D(new Vector3D(turnedFloorTranslate.OffsetX, turnedFloorTranslate.OffsetY + PackageHeight * 2 * i + i * 0.01, turnedFloorTranslate.OffsetZ));
                }

                var coloredFloors = new Model3DGroup();
                coloredFloors = ThrowUpRainbow(packageFloors);
                mainModel.Children.Add(coloredFloors);
            }
            return mainModel;
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



        private Model3DGroup FloorGenerator (FloorMap floorMap, GeometryModel3D model, int whichLevel)
        {
            Model3DGroup floor = new Model3DGroup();
            foreach (MapNode mapNode in floorMap.Map)
            {
                GeometryModel3D geometryModel = new GeometryModel3D();
                geometryModel = model.Clone();
                RotateTransform3D rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90));
                Transform3DGroup transformation = new Transform3DGroup();

                if (mapNode.IsTurned)
                    transformation.Children.Add(rotation);

                TranslateTransform3D translation = new TranslateTransform3D(new Vector3D(mapNode.PosX, PackageHeight, -mapNode.PosY));
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

                Transform3DGroup transformGroup = (Transform3DGroup) geometryModel.Transform;
                TranslateTransform3D translation = (TranslateTransform3D)transformGroup.Children[transformGroup.Children.Count - 1];

                double minCandidateX, maxCandidateX, minCandidateZ, maxCandidateZ;

                if (transformGroup.Children.Count > 1)
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
                Model3DGroup signsAll = new Model3DGroup();
                modelGroup.Children.Add(signsAll);
                signsAll.Transform = transformGroup;
                bool turned = transformGroup.Children.Count < 2 ? true : false;
                for (int k = 0; k < 5; k++)
                {
                    signsAll.Children.Add(new GeometryModel3D());
                    if (k == 4)
                        signsAll.Children[k] = SignUp(whichLevel * floor.Children.Count + i + 1);
                    if (k == 3)
                        signsAll.Children[k] = SignFront(whichLevel * floor.Children.Count + i + 1, turned);
                    if (k == 2)
                        signsAll.Children[k] = SignBack(whichLevel * floor.Children.Count + i + 1, turned);
                    if (k == 1)
                        signsAll.Children[k] = SignRight(whichLevel * floor.Children.Count + i + 1, turned);
                    if (k == 0)
                        signsAll.Children[k] = SignLeft(whichLevel * floor.Children.Count + i + 1, turned);


                }
                //modelGroup.Children.Add(SignGenerator(15, false));
                //modelGroup.Children[1].Transform = modelGroup.Children[0].Transform;
                floor.Children[i] = modelGroup;
            }

            floor.Transform = new TranslateTransform3D(new Vector3D( - (max_X - (max_X - min_X) / 2), 0, -(max_Z - (max_Z - min_Z) / 2)));
            return floor;
        }



        private Model3DGroup ThrowUpRainbow (Model3DGroup sadFloors)
        {
            ColourGenerator colourGenerator = new ColourGenerator( ((Model3DGroup)sadFloors.Children[0]).Children.Count );

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
                }
            }
            
            return sadFloors;
        }



        private GeometryModel3D SignUp(int number)
        {
            string text = number.ToString();
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };


            VisualBrush visualBrush = new VisualBrush(textBlock);
            RenderOptions.SetCachingHint(visualBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(visualBrush, 0);
            RenderOptions.SetCacheInvalidationThresholdMaximum(visualBrush, double.MaxValue);


            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = visualBrush,
            };

            double minDimension = PackageWidth < PackageLength ? PackageWidth : PackageLength;

            minDimension /= 2;


            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(-minDimension,  0.005, -minDimension),
                new Point3D(-minDimension,  0.005,  minDimension),
                new Point3D( minDimension,  0.005,  minDimension),
                new Point3D( minDimension,  0.005, -minDimension),
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,1,2,
                2,3,0,
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
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

            textModel.Transform = new TranslateTransform3D(new Vector3D(0, PackageHeight, 0));

            return textModel;
        }



        private GeometryModel3D SignDown(int number)
        {
            string text = number.ToString();
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };


            VisualBrush visualBrush = new VisualBrush(textBlock);
            RenderOptions.SetCachingHint(visualBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(visualBrush, 0);
            RenderOptions.SetCacheInvalidationThresholdMaximum(visualBrush, double.MaxValue);


            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = visualBrush,
            };

            double minDimension = PackageWidth < PackageLength ? PackageWidth : PackageLength;

            minDimension /= 2;


            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(-minDimension,  -0.005, -minDimension),
                new Point3D(-minDimension,  -0.005,  minDimension),
                new Point3D( minDimension,  -0.005,  minDimension),
                new Point3D( minDimension,  -0.005, -minDimension),
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,3,2,
                2,1,0,
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(0, 1),
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
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

            textModel.Transform = new TranslateTransform3D(new Vector3D(0, -PackageHeight, 0));

            return textModel;
        }



        private GeometryModel3D SignFront(int number, bool turned)
        {
            string text = number.ToString();
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };


            VisualBrush visualBrush = new VisualBrush(textBlock);
            RenderOptions.SetCachingHint(visualBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(visualBrush, 0);
            RenderOptions.SetCacheInvalidationThresholdMaximum(visualBrush, double.MaxValue);


            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = visualBrush,
            };

            double minDimension = 0;
            if (turned == false)
            {
                minDimension = PackageWidth < PackageHeight ? PackageWidth : PackageHeight;
            }
            else
            {
                minDimension = PackageLength < PackageHeight ? PackageLength : PackageHeight;
            }
            minDimension /= 2;


            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(-minDimension, minDimension,  0.005),
                new Point3D(-minDimension, -minDimension, 0.005),
                new Point3D(minDimension,  -minDimension, 0.005),
                new Point3D(minDimension,  minDimension,  0.005)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,1,2,
                2,3,0
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0)
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

            
            textModel.Transform = new TranslateTransform3D(new Vector3D(0, 0, PackageLength));

            return textModel;
        }



        private GeometryModel3D SignBack(int number, bool turned)
        {
            string text = number.ToString();
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };


            VisualBrush visualBrush = new VisualBrush(textBlock);
            RenderOptions.SetCachingHint(visualBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(visualBrush, 0);
            RenderOptions.SetCacheInvalidationThresholdMaximum(visualBrush, double.MaxValue);


            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = visualBrush,
            };

            double minDimension = 0;
            if (turned == false)
            {
                minDimension = PackageWidth < PackageHeight ? PackageWidth : PackageHeight;
            }
            else
            {
                minDimension = PackageLength < PackageHeight ? PackageLength : PackageHeight;
            }
            minDimension /= 2;


            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(-minDimension, minDimension,  -0.005),
                new Point3D(-minDimension, -minDimension, -0.005),
                new Point3D(minDimension,  -minDimension, -0.005),
                new Point3D(minDimension,  minDimension,  -0.005)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,3,2,
                2,1,0
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),
                new Point(0, 0),
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

            textModel.Transform = new TranslateTransform3D(new Vector3D(0, 0, -PackageLength));

            return textModel;
        }



        private GeometryModel3D SignRight(int number, bool turned)
        {
            string text = number.ToString();
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };


            VisualBrush visualBrush = new VisualBrush(textBlock);
            RenderOptions.SetCachingHint(visualBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(visualBrush, 0);
            RenderOptions.SetCacheInvalidationThresholdMaximum(visualBrush, double.MaxValue);


            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = visualBrush,
            };

            double minDimension = 0;
            if (turned == false)
            {
                minDimension = PackageLength < PackageHeight ? PackageLength : PackageHeight;
            }
            else
            {
                minDimension = PackageWidth < PackageHeight ? PackageWidth : PackageHeight;
            }
            minDimension /= 2;


            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(0.005,  minDimension,  minDimension),
                new Point3D(0.005, -minDimension,  minDimension),
                new Point3D(0.005, -minDimension, -minDimension),
                new Point3D(0.005,  minDimension, -minDimension)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,1,2,
                2,3,0
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
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

            textModel.Transform = new TranslateTransform3D(new Vector3D(PackageWidth, 0, 0));

            return textModel;
        }



        private GeometryModel3D SignLeft(int number, bool turned)
        {
            string text = number.ToString();
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.White,
                Background = Brushes.Black,
                FontFamily = new FontFamily("Forte")
            };


            VisualBrush visualBrush = new VisualBrush(textBlock);
            RenderOptions.SetCachingHint(visualBrush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(visualBrush, 0);
            RenderOptions.SetCacheInvalidationThresholdMaximum(visualBrush, double.MaxValue);


            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = visualBrush,
            };

            double minDimension = 0;
            if (turned == false)
            {
                minDimension = PackageLength < PackageHeight ? PackageLength : PackageHeight;
            }
            else
            {
                minDimension = PackageWidth < PackageHeight ? PackageWidth : PackageHeight;
            }
            minDimension /= 2;


            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(-0.005,  minDimension,  minDimension),
                new Point3D(-0.005, -minDimension,  minDimension),
                new Point3D(-0.005, -minDimension, -minDimension),
                new Point3D(-0.005,  minDimension, -minDimension)
            };

            Int32Collection triangleIndices = new Int32Collection
            {
                0,3,2,
                2,1,0
            };

            PointCollection textureCoordinates = new PointCollection
            {
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),
                new Point(0, 0),
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

            textModel.Transform = new TranslateTransform3D(new Vector3D(-PackageWidth, 0, 0));

            return textModel;
        }



    }
}