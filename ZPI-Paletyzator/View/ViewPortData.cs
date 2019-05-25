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
        public ICommand GetPanelSizeCommand => _getPanelSizeCommand;
        public ICommand MoveCommand => _moveCommand;
        public ICommand LeftButtonDownCommand => _leftButtonDownCommand;
        public ICommand LeftButtonReleaseCommand => _leftButtonReleaseCommand;
        public ICommand RightButtonDownCommand => _rightButtonDownCommand;
        public ICommand RightButtonReleaseCommand => _rightButtonReleaseCommand;

        private readonly RelayCommand _getPanelSizeCommand;
        private readonly RelayCommand _moveCommand;
        private readonly RelayCommand _leftButtonDownCommand;
        private readonly RelayCommand _leftButtonReleaseCommand;
        private readonly RelayCommand _rightButtonDownCommand;
        private readonly RelayCommand _rightButtonReleaseCommand;
        private double Pix2AngleX { get; set; }
        private double Pix2AngleY { get; set; }
        private double PositionOldX { get; set; }
        private double PositionOldY { get; set; }
        private double AngleX { get; set; }
        private double AngleY { get; set; }
        private double AngleCamera { get; set; }
        private bool MouseLeftButtonStatus { get; set; }
        private bool MouseRightButtonStatus { get; set; }

        private TranslateTransform3D ModelTranslation;


        public ViewPortData()
        {
            _getPanelSizeCommand = new RelayCommand(GetPanelSize, obj => true);
            _moveCommand = new RelayCommand(MouseMove, obj => true);
            _leftButtonDownCommand = new RelayCommand(MouseLeftButtonDown, obj => true);
            _leftButtonReleaseCommand = new RelayCommand(MouseLeftButtonRelease, obj => true);
            _rightButtonDownCommand = new RelayCommand(MouseRightButtonDown, obj => true);
            _rightButtonReleaseCommand = new RelayCommand(MouseRightButtonRelease, obj => true);

            AngleCamera = 10;
            MainCamera = new PerspectiveCamera()
            {
                Position = new Point3D(0, 0, AngleCamera),
                LookDirection = new Vector3D(0, 0, -1),
                FieldOfView = 100
            };

            var myAmbientLight = new AmbientLight(Colors.White);
            LightModel = new Model3DGroup();
            LightModel.Children.Add(myAmbientLight);

            ModelSource = new Model3DGroup();
            ModelSource.Children.Add(PaletteGenerator.FlatPartGenerator());
            ModelSource.Children.Add(PaletteGenerator.GroundPartsGenerator());
            ModelTranslation = new TranslateTransform3D(0, -2, 0);
            ModelSource.Transform = ModelTranslation;
        }

        private void GetPanelSize(object obj)
        {
            if (obj is Panel panObj)
            {
                Pix2AngleX = 360 / panObj.ActualWidth;
                Pix2AngleY = 360 / panObj.ActualHeight;
            }
        }

        private void Resize(object obj)
        {
            if (obj is Panel panObj)
            {
                Pix2AngleX = 360 / panObj.ActualWidth;
                Pix2AngleY = 360 / panObj.ActualHeight;
            }
        }

        private void MouseMove(object obj)
        {
            if (obj is Panel PanelObject)
            {
                double deltaX = Mouse.GetPosition(PanelObject).X - PositionOldX;
                double deltaY = Mouse.GetPosition(PanelObject).Y - PositionOldY;
                PositionOldX = Mouse.GetPosition(PanelObject).X;
                PositionOldY = Mouse.GetPosition(PanelObject).Y;

                if (MouseLeftButtonStatus)
                {
                    AngleX += deltaX * Pix2AngleX;
                    AngleY += deltaY * Pix2AngleY;
                }

                if (MouseRightButtonStatus)
                {
                    AngleCamera += deltaY * Pix2AngleY / 5;
                }

                if (AngleX < 0)
                    AngleX += 360;
                else if (AngleX > 360)
                    AngleX -= 360;

                if (AngleY < 0)
                    AngleY = 0;
                else if (AngleY > 30)
                    AngleY = 30;

                if (AngleCamera < 10)
                    AngleCamera = 10;
                else if (AngleCamera > 15)
                    AngleCamera = 15;

                if (MouseLeftButtonStatus)
                {
                    var TransformGroup = new Transform3DGroup();
                    TransformGroup.Children.Add(ModelTranslation);
                    TransformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), AngleX)));
                    TransformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), AngleY)));
                    ModelSource.Transform = TransformGroup;
                }

                if (MouseRightButtonStatus)
                {
                    MainCamera.Position = new Point3D(0, 0, AngleCamera);
                }
            }
        }

        private void MouseLeftButtonDown(object obj)
        {
            MouseLeftButtonStatus = true;
        }

        private void MouseLeftButtonRelease(object obj)
        {
            MouseLeftButtonStatus = false;
        }

        private void MouseRightButtonDown(object obj)
        {
            MouseRightButtonStatus = true;
        }

        private void MouseRightButtonRelease(object obj)
        {
            MouseRightButtonStatus = false;
        }

    }
}
