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
        public static GeometryModel3D FlatPartGenerator()
        {
            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(-3, 0, -4.5),
                new Point3D(-3, 0, 4.5),
                new Point3D(3, 0, 4.5),
                new Point3D(3, 0, -4.5),

                new Point3D(-3, -0.4, -4.5),
                new Point3D(-3, -0.4, 4.5),
                new Point3D(3, -0.4, 4.5),
                new Point3D(3, -0.4, -4.5)
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



        public static Model3DGroup GroundPartsGenerator()
        {

            var groundGroup = new Model3DGroup();

            Point3DCollection meshPoints = new Point3DCollection
            {
                new Point3D(-3, -0.4, -4.5),
                new Point3D(-3, -0.4, 4.5),
                new Point3D(-2, -0.4, 4.5),
                new Point3D(-2, -0.4, -4.5),

                new Point3D(-3, -0.8, -4.5),
                new Point3D(-3, -0.8, 4.5),
                new Point3D(-2, -0.8, 4.5),
                new Point3D(-2, -0.8, -4.5)
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


            groundGroup.Children[1].Transform = new TranslateTransform3D(2.5, 0, 0);
            groundGroup.Children[2].Transform = new TranslateTransform3D(5, 0, 0);

            return groundGroup;
        }



        public static GeometryModel3D Ground()
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



        public static Model3DGroup Sign()
        {
            string text = "Noooo siema...";
            TextBlock textBlock = new TextBlock(new Run(text))
            {
                Foreground = Brushes.Pink,
                FontFamily = new FontFamily("Forte")
            };

            DiffuseMaterial material = new DiffuseMaterial()
            {
                Brush = new VisualBrush(textBlock)
            };

            Point3DCollection textPointCollection = new Point3DCollection
            {
                new Point3D(-10, 10, 0),
                new Point3D(-10, 5, 0),
                new Point3D(10, 5, 0),
                new Point3D(10, 10, 0)
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




            GeometryModel3D backTextModel = textModel.Clone();
            backTextModel.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180));

            var group = new Model3DGroup();
            group.Children.Add(textModel);
            group.Children.Add(backTextModel);
            

            return group;
        }
    }
}
