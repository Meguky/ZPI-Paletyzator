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
        public PerspectiveCamera MainCamera { get; set; }
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

        public MouseControlCamera MouseControlCamera { get; set; }

        private Action<PerspectiveCamera> CameraUpdate;

        public ViewPortData()
        {
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
            var myAmbientLight = new AmbientLight(Colors.White);
            LightModel.Children.Add(myAmbientLight);

            ModelSource = new Model3DGroup();
            ModelSource.Children.Add(SceneObjectsGenerator.FlatPartGenerator());
            ModelSource.Children.Add(SceneObjectsGenerator.GroundPartsGenerator());
            ModelSource.Children.Add(SceneObjectsGenerator.Ground());
            ModelSource.Children.Add(SceneObjectsGenerator.Sign());

            Transform3DGroup TranslationGroup = new Transform3DGroup();
            TranslationGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            TranslationGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, -4, 0)));
            ModelSource.Transform = TranslationGroup;
        }
    }
}
