using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;


namespace ZPI_Paletyzator.View
{
    class PaletteGenerator
    {
        public static GeometryModel3D FlatPartGenerator()
        {
            Point3DCollection palletFlatMeshPoints = new Point3DCollection
            {
                new Point3D(-3, 0, -4),
                new Point3D(-3, 0, 4),
                new Point3D(3, 0, 4),
                new Point3D(3, 0, -4),

                new Point3D(-3, -0.4, -4),
                new Point3D(-3, -0.4, 4),
                new Point3D(3, -0.4, 4),
                new Point3D(3, -0.4, -4)
            };

            Int32Collection triangleIndicesFlat = new Int32Collection
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
                Positions = palletFlatMeshPoints,
                TriangleIndices = triangleIndicesFlat
            };

            GeometryModel3D palletFlat = new GeometryModel3D
            {
                Geometry = palletFlatMesh,            
            };

            SolidColorBrush flatBrush = new SolidColorBrush(Colors.Tan);
            palletFlat.Material = new DiffuseMaterial(flatBrush);

            return palletFlat;
        }



        public static Model3DGroup GroundPartsGenerator()
        {

            var groundGroup = new Model3DGroup();

            Point3DCollection palletGroundPartMeshPoints = new Point3DCollection
            {
                new Point3D(-3, -0.4, -4),
                new Point3D(-3, -0.4, 4),
                new Point3D(-2, -0.4, 4),
                new Point3D(-2, -0.4, -4),

                new Point3D(-3, -0.8, -4),
                new Point3D(-3, -0.8, 4),
                new Point3D(-2, -0.8, 4),
                new Point3D(-2, -0.8, -4)
            };

            Int32Collection triangleIndicesGroundPart = new Int32Collection
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

            MeshGeometry3D palletGroundPartMesh = new MeshGeometry3D
            {
                Positions = palletGroundPartMeshPoints,
                TriangleIndices = triangleIndicesGroundPart
            };
            GeometryModel3D palletGroundPart = new GeometryModel3D
            {
                Geometry = palletGroundPartMesh
            };

            var GroundBrush = new SolidColorBrush(Colors.Bisque);
            palletGroundPart.Material = new DiffuseMaterial(GroundBrush);


            groundGroup.Children.Add(palletGroundPart);
            groundGroup.Children.Add(palletGroundPart.Clone());
            groundGroup.Children.Add(palletGroundPart.Clone());


            groundGroup.Children[1].Transform = new TranslateTransform3D(2.5, 0, 0);
            groundGroup.Children[2].Transform = new TranslateTransform3D(5, 0, 0);

            return groundGroup;
        }



        public static GeometryModel3D Ground()
        {
            Point3DCollection groundMeshPoints = new Point3DCollection
            {
                new Point3D(-30, -0.8, -30),
                new Point3D(-30, -0.8, 30),
                new Point3D(30, -0.8, 30),
                new Point3D(30, -0.8, -30)
            };

            Int32Collection triangleIndicesFlat = new Int32Collection
            {
                0,1,2,
                2,3,0,
                0,4,5,
                5,1,0,
            };


            MeshGeometry3D groundMesh = new MeshGeometry3D
            {
                Positions = groundMeshPoints,
                TriangleIndices = triangleIndicesFlat
            };

            GeometryModel3D groundFlat = new GeometryModel3D
            {
                Geometry = groundMesh
            };

            SolidColorBrush groundBrush = new SolidColorBrush(Colors.YellowGreen);
            groundFlat.Material = new DiffuseMaterial(groundBrush);

            return groundFlat;
        }
    }
}
