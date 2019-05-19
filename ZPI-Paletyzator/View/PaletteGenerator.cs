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
                4,0,3
            };

            MeshGeometry3D palletFlatMesh = new MeshGeometry3D()
            {
                Positions = palletFlatMeshPoints,
                TriangleIndices = triangleIndicesFlat
            };

            GeometryModel3D palletFlat = new GeometryModel3D()
            {
                Geometry = palletFlatMesh
            };

            SolidColorBrush FlatBrush = new SolidColorBrush(Colors.Tan);
            DiffuseMaterial FlatMaterial = new DiffuseMaterial(FlatBrush);
            palletFlat.Material = palletFlat.BackMaterial = FlatMaterial;

            return palletFlat;
        }

        public static Model3DGroup GroundPartsGenerator()
        {

            Model3DGroup groundGroup = new Model3DGroup();

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

            MeshGeometry3D palletGroundPartMesh = new MeshGeometry3D()
            {
                Positions = palletGroundPartMeshPoints,
                TriangleIndices = triangleIndicesGroundPart
            };
            GeometryModel3D palletGroundPart = new GeometryModel3D()
            {
                Geometry = palletGroundPartMesh
            };

            SolidColorBrush GroundBrush = new SolidColorBrush(Colors.Bisque);
            DiffuseMaterial GroundEMaterial = new DiffuseMaterial(GroundBrush);
            palletGroundPart.Material = palletGroundPart.BackMaterial = GroundEMaterial;


            TranslateTransform3D trans2NdPart = new TranslateTransform3D(2.5, 0, 0);
            TranslateTransform3D trans3RdPart = new TranslateTransform3D(5, 0, 0);

            groundGroup.Children.Add(palletGroundPart);
            groundGroup.Children.Add(palletGroundPart.Clone());
            groundGroup.Children.Add(palletGroundPart.Clone());
            groundGroup.Children[1].Transform = trans2NdPart;
            groundGroup.Children[2].Transform = trans3RdPart;

            return groundGroup;
        }
    }
}
