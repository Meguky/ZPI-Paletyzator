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
    class ViewPortData
    {
        public PerspectiveCamera MainCamera { get; private set; }
        public Model3DGroup LightModel { get; private set; }
        public Model3DGroup ModelSource { get; private set; }
        public int Levels { get; private set; }
        private int _focusedLevel = 0;
        private bool _isFocusOn = false;

        public ICommand InitPanelCommand => _initPanelCommand;
        public ICommand GetPanelSizeCommand => _getPanelSizeCommand;
        public ICommand MoveCommand => _moveCommand;
        public ICommand LeftButtonDownCommand => _leftButtonDownCommand;
        public ICommand LeftButtonReleaseCommand => _leftButtonReleaseCommand;
        public ICommand RightButtonDownCommand => _rightButtonDownCommand;
        public ICommand RightButtonReleaseCommand => _rightButtonReleaseCommand;
        public ICommand SlideChangeValue => _slideChangeValue;
        public ICommand IsLevelFocusOn => _isLevelFocusOn;

        private readonly RelayCommand _initPanelCommand;
        private readonly RelayCommand _getPanelSizeCommand;
        private readonly RelayCommand _moveCommand;
        private readonly RelayCommand _leftButtonDownCommand;
        private readonly RelayCommand _leftButtonReleaseCommand;
        private readonly RelayCommand _rightButtonDownCommand;
        private readonly RelayCommand _rightButtonReleaseCommand;
        private readonly RelayCommand _slideChangeValue;
        private readonly RelayCommand _isLevelFocusOn;

        private double PackageHeight { get; set; }
        private double PackageWidth { get; set; }
        private double PackageLength { get; set; }
        private double PaletteWidth { get; set; }
        private double PaletteLength { get; set; }

        private MouseControlCamera MouseControlCamera { get; set; }

        private Action<PerspectiveCamera> CameraUpdate;

        private PackagesGenerator packagesGenerator;
        private PaletteBase paletteBase;

        public ViewPortData()
        {
            ModelSource = new Model3DGroup();
            MainCamera = new PerspectiveCamera();
            CameraUpdate = (PerspectiveCamera Camera) =>
            {
                MainCamera.Position = Camera.Position;
                MainCamera.LookDirection = Camera.LookDirection;
                MainCamera.FieldOfView = Camera.FieldOfView;
                TurnSigns(MainCamera.Position);
            };

            MouseControlCamera = new MouseControlCamera(CameraUpdate);

            _initPanelCommand = new RelayCommand(MouseControlCamera.InitPanel, obj => true);
            _getPanelSizeCommand = new RelayCommand(MouseControlCamera.GetPanelSize, obj => true);
            _moveCommand = new RelayCommand(MouseControlCamera.MouseMove, obj => true);
            _leftButtonDownCommand = new RelayCommand(MouseControlCamera.MouseLeftButtonDown, obj => true);
            _leftButtonReleaseCommand = new RelayCommand(MouseControlCamera.MouseLeftButtonRelease, obj => true);
            _rightButtonDownCommand = new RelayCommand(MouseControlCamera.MouseRightButtonDown, obj => true);
            _rightButtonReleaseCommand = new RelayCommand(MouseControlCamera.MouseRightButtonRelease, obj => true);
            _slideChangeValue = new RelayCommand(SlideValue, obj => true);
            _isLevelFocusOn = new RelayCommand(LevelFocusOn, obj => true);

            LightModel = new Model3DGroup();
            LightModel.Children.Add(new AmbientLight(Colors.White));


            paletteBase = new PaletteBase(0, 0, 0, 0, 0);
            ModelSource.Children.Add(paletteBase.GetModel());

            Transform3DGroup TranslationGroup = new Transform3DGroup();
            TranslationGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            TranslationGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 0)));
            ModelSource.Transform = TranslationGroup;
        }



        public void AddSceneObjects(double packageHeight, double packageWidth, double packageLength, double paletteWidth, double paletteLength, int levels = 1)
        {
            Levels = levels;
            PackageHeight = packageHeight;
            PackageWidth = packageWidth;
            PackageLength = packageLength;
            PaletteWidth = paletteWidth;
            PaletteLength = paletteLength;

            ModelSource = new Model3DGroup();
            paletteBase = new PaletteBase(packageHeight, packageWidth, packageLength, paletteWidth, paletteLength);
            ModelSource.Children.Add(paletteBase.GetModel());
            packagesGenerator = new PackagesGenerator(packageHeight, packageWidth, packageLength, paletteWidth, paletteLength, Levels);
            ModelSource.Children.Add(packagesGenerator.GetModel());

            Transform3DGroup TranslationGroup = new Transform3DGroup();
            TranslationGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            TranslationGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 0)));
            ModelSource.Transform = TranslationGroup;

            ChangeFocus();
        }



        private void TurnSigns (Point3D point)
        {
            if (ModelSource.Children.Count > 1)
            {
                for (int i = 0; i < packagesGenerator.upSignShortcut.Count; i++)
                {
                    var geometryModel = new GeometryModel3D();
                    geometryModel = packagesGenerator.upSignShortcut[i];
                    double arcCosinus = Math.Acos(point.X / Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Z, 2)));
                    arcCosinus *= 180 / Math.PI;

                    if (point.Z > 0)
                        arcCosinus *= -1;
                    if ((i / packagesGenerator.PackagesPerFloor) % 2 == 1)
                        arcCosinus -= 90;

                    var rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,1,0), arcCosinus ));
                    ((Transform3DGroup)geometryModel.Transform).Children[0] = rotation;
                }
            }

        }



        private void ChangeFocus()
        {
            if (_isFocusOn)
            {
                for (int i = 0; i < Levels; i++)
                {
                    var floor = new Model3DGroup();
                    floor = (Model3DGroup)((Model3DGroup)ModelSource.Children[1]).Children[i];
                    var translation = new TranslateTransform3D();
                    translation = (TranslateTransform3D)floor.Transform;

                    if (i <= _focusedLevel)
                    {
                        translation.OffsetY = 2 * i * PackageHeight * paletteBase.GetVisualMilimeter();
                    }
                    else
                    {
                        translation.OffsetY = 2 * i * PackageHeight * paletteBase.GetVisualMilimeter() + 15;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Levels; i++)
                {
                    var floor = new Model3DGroup();
                    floor = (Model3DGroup)((Model3DGroup)ModelSource.Children[1]).Children[i];
                    var translation = new TranslateTransform3D();
                    translation = (TranslateTransform3D)floor.Transform;
                    translation.OffsetY = 2 * i * PackageHeight * paletteBase.GetVisualMilimeter();
                }
            }
        }



        private void SlideValue (object obj)
        {
            if (obj is Slider slider && Levels > 0)
            {
                _focusedLevel = (int)slider.Value;
                Transform3DGroup transformGroup = new Transform3DGroup();
                transformGroup = (Transform3DGroup) ModelSource.Transform;
                TranslateTransform3D translation = new TranslateTransform3D();
                translation = (TranslateTransform3D)transformGroup.Children[1];
                translation.OffsetY = -2 * _focusedLevel * PackageHeight * paletteBase.GetVisualMilimeter();

                ChangeFocus();
            }
        }
        


        private void LevelFocusOn (object obj)
        {
            _isFocusOn = !_isFocusOn;

            ChangeFocus();
        }



        
    }
}