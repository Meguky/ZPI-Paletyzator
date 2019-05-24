using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ZPI_Paletyzator.View;

using System.Windows.Media.Animation;

namespace ZPI_Paletyzator.View
{
    class ViewPortData
    {
        public PerspectiveCamera MainCamera { get; private set; }
        public Model3DGroup LightModel { get; private set; }
        public Model3DGroup ModelSource { get; private set; }

        public ViewPortData()
        {
            MainCamera = new PerspectiveCamera()
            {
                Position = new Point3D(-2, 4, 7),
                LookDirection = new Vector3D(0.3, -0.7, -1),
                FieldOfView = 100
            };

            var myAmbientLight = new AmbientLight(Colors.White);
            LightModel = new Model3DGroup();
            LightModel.Children.Add(myAmbientLight);

            ModelSource = new Model3DGroup();
            ModelSource.Children.Add(PaletteGenerator.FlatPartGenerator());
            ModelSource.Children.Add(PaletteGenerator.GroundPartsGenerator());


            var myRotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,1,0), 20));
            //var myRotateTransform_Z = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -50));

            //ModelSource.Transform = myRotateTransform;

            //Transform3DGroup myTrangsGroup = new Transform3DGroup();
            //myTrangsGroup.Children.Add(myRotateTransform);
            //myTrangsGroup.Children.Add(myRotateTransform_Z);
            //ModelSource.Transform = myTrangsGroup;
        }
    }
}
