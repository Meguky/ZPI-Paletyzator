using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ZPI_Paletyzator.View
{
    class ViewPortInit
    {
        public PerspectiveCamera MainCamera { get; private set; }
        public Model3DGroup LightModel { get; private set; }
        public Model3DGroup ModelSource { get; private set; }

        public ViewPortInit()
        {
            MainCamera = new PerspectiveCamera()
            {
                Position = new Point3D(-2, 3, 7),
                LookDirection = new Vector3D(0.3, -0.7, -1),
                FieldOfView = 90
            };

            AmbientLight myAmbientLight = new AmbientLight(Colors.White);
            LightModel = new Model3DGroup();
            LightModel.Children.Add(myAmbientLight);

            ModelSource = new Model3DGroup();
            ModelSource.Children.Add(View.PaletteGenerator.FlatPartGenerator());
            ModelSource.Children.Add(View.PaletteGenerator.GroundPartsGenerator());
        }
    }
}
