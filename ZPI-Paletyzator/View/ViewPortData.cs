using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ZPI_Paletyzator.View;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ZPI_Paletyzator.View
{
    sealed class ViewPortData
    {
        public PerspectiveCamera MainCamera { get; private set; }
        public Model3DGroup LightModel { get; private set; }
        public Model3DGroup ModelSource { get; private set; }
        public ICommand InitPanelCommand => _initPanelCommand;
        public ICommand GetPanelSizeCommand => _getPanelSizeCommand;
        public ICommand MoveCommand => _moveCommand;
        public ICommand LeftButtonDownCommand => _leftButtonDownCommand;
        public ICommand LeftButtonReleaseCommand => _leftButtonReleaseCommand;
        public ICommand RightButtonDownCommand => _rightButtonDownCommand;
        public ICommand RightButtonReleaseCommand => _rightButtonReleaseCommand;

        private readonly RelayCommand _initPanelCommand;
        private readonly RelayCommand _getPanelSizeCommand;
        private readonly RelayCommand _moveCommand;
        private readonly RelayCommand _leftButtonDownCommand;
        private readonly RelayCommand _leftButtonReleaseCommand;
        private readonly RelayCommand _rightButtonDownCommand;
        private readonly RelayCommand _rightButtonReleaseCommand;

        private double PackageHeight { get; set; }
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }

        private MouseControlCamera MouseControlCamera { get; set; }

        private Action<PerspectiveCamera> CameraUpdate;



        public ViewPortData(double packageHeight = 0, double packageWidth = 0, double packageLength = 0, double paletteWidth = 0, double paletteLength = 0)
        {
            PackageHeight = packageHeight;
            PackageWidth = packageWidth;
            PackageLength = packageLength;
            PaletteWidth = paletteWidth;
            PaletteLength = paletteLength;

            MainCamera = new PerspectiveCamera();
            CameraUpdate = (PerspectiveCamera Camera) =>
            {
                MainCamera.Position = Camera.Position;
                MainCamera.LookDirection = Camera.LookDirection;
                MainCamera.FieldOfView = Camera.FieldOfView;
            };

            MouseControlCamera = new MouseControlCamera(CameraUpdate);

            _initPanelCommand = new RelayCommand(MouseControlCamera.InitPanel, obj => true);
            _getPanelSizeCommand = new RelayCommand(MouseControlCamera.GetPanelSize, obj => true);
            _moveCommand = new RelayCommand(MouseControlCamera.MouseMove, obj => true);
            _leftButtonDownCommand = new RelayCommand(MouseControlCamera.MouseLeftButtonDown, obj => true);
            _leftButtonReleaseCommand = new RelayCommand(MouseControlCamera.MouseLeftButtonRelease, obj => true);
            _rightButtonDownCommand = new RelayCommand(MouseControlCamera.MouseRightButtonDown, obj => true);
            _rightButtonReleaseCommand = new RelayCommand(MouseControlCamera.MouseRightButtonRelease, obj => true);

            LightModel = new Model3DGroup();
            LightModel.Children.Add(new AmbientLight(Colors.White));

            ModelSource = new Model3DGroup();
            PaletteBase packagesGenerator = new PaletteBase(packageHeight, packageWidth, packageLength, paletteWidth, paletteLength);
            ModelSource.Children.Add(packagesGenerator.GetModel());

            Transform3DGroup TranslationGroup = new Transform3DGroup();
            TranslationGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            TranslationGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, -4, 0)));
            ModelSource.Transform = TranslationGroup;
        }



        public void AddSceneObjects(double packageHeight, double packageWidth, double packageLength, double paletteWidth, double paletteLength, int levels = 1)
        {
            PackageHeight = packageHeight;
            PackageWidth = packageWidth;
            PackageLength = packageLength;
            PaletteWidth = paletteWidth;
            PaletteLength = paletteLength;

            ModelSource = new Model3DGroup();
            PaletteBase paletteBase = new PaletteBase(packageHeight, packageWidth, packageLength, paletteWidth, paletteLength);
            ModelSource.Children.Add(paletteBase.GetModel());
            PackagesGenerator packagesGenerator = new PackagesGenerator(packageHeight, packageWidth, packageLength, paletteWidth, paletteLength, levels);
            ModelSource.Children.Add(packagesGenerator.GetModel());

            Transform3DGroup TranslationGroup = new Transform3DGroup();
            TranslationGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            TranslationGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, -4, 0)));
            ModelSource.Transform = TranslationGroup;
        }
    }
}
