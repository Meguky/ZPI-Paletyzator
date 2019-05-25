using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ZPI_Paletyzator.View;
using System.Windows.Input;
using System.Windows.Controls;

namespace ZPI_Paletyzator.View
{
    class MouseControlCamera
    {
        private double Pix2AngleX { get; set; }
        private double Pix2AngleY { get; set; }
        private double PositionOldX { get; set; }
        private double PositionOldY { get; set; }
        private double AngleX { get; set; }
        private double AngleY { get; set; }
        private bool MouseLeftButtonStatus { get; set; }
        private bool MouseRightButtonStatus { get; set; }

        private double CameraPositionX { get; set; }
        private double CameraPositionY { get; set; }
        private double CameraPositionZ { get; set; }
        private double CameraR { get; set; }
        private double MaxCameraR { get; set; }
        private double MinCameraR { get; set; }
        
        private PerspectiveCamera Camera { get; set; }

        Action<PerspectiveCamera> CopyCamera;

        private double OldPanelWidth { get; set; }

        private double AxisY { get; set; }
        private double MaxAxisY { get; set; }
        private double MinAxisY { get; set; }

        public MouseControlCamera (Action<PerspectiveCamera> getMainCamera)
        {
            AxisY = MinAxisY = 3;
            MaxAxisY = 10;
            CameraR = MinCameraR = 20;
            MaxCameraR = 35;
            Camera = new PerspectiveCamera()
            {
                Position = new Point3D(CameraR, 0, 0),
                LookDirection = new Vector3D(-CameraR, 0, 0),
                FieldOfView = 60
            };
            CopyCamera = getMainCamera;
            Draw();
        }


        public void InitPanel(object obj)
        {
            if (obj is Panel panObj)
            {
                Pix2AngleX = 360 * 0.01 / panObj.ActualWidth;
                Pix2AngleY = 360 * 0.01 / panObj.ActualHeight;
                OldPanelWidth = panObj.ActualWidth;
            }
        }

        public void GetPanelSize(object obj)
        {
            if (obj is Panel panObj)
            {
                Pix2AngleX = 360 * 0.01 / panObj.ActualWidth;
                Pix2AngleY = 360 * 0.01 / panObj.ActualHeight;

                if (panObj.ActualWidth != OldPanelWidth)
                {
                    double deltaWidth = panObj.ActualWidth - OldPanelWidth;
                    double x = 116;
                    CameraR += deltaWidth / x; // (1660 - 500) / x; 
                    MinCameraR += deltaWidth / x;
                    MaxCameraR += deltaWidth / x;

                    if (CameraR > MaxCameraR)
                        CameraR = MaxCameraR;
                    else if (CameraR < MinCameraR)
                        CameraR = MinCameraR;

                    AxisY -= deltaWidth / (1160 / 2);
                }

                Draw();
                OldPanelWidth = panObj.ActualWidth;
            }
        }

        public void MouseMove(object obj)
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

                    if (AngleX < -Math.PI * 2)
                        AngleX = 0;
                    else if (AngleX > Math.PI * 2)
                        AngleX = 0;

                    if (AngleY < 0)
                        AngleY = 0;
                    else if (AngleY > Math.PI / 4)
                        AngleY = Math.PI / 4;

                }


                if (MouseRightButtonStatus)
                {
                    double newCameraR = CameraR + (deltaY * Pix2AngleY) * 10;
                    if (newCameraR > MinCameraR && newCameraR < MaxCameraR)
                    {
                        double delta = (MaxCameraR - MinCameraR) / (newCameraR - CameraR);
                        CameraR = newCameraR;
                        AxisY += (MaxAxisY - MinAxisY) / delta;
                    }
                }

                if (MouseLeftButtonStatus || MouseRightButtonStatus)
                {
                    Draw();
                }
            }
        }

        private void Draw()
        {
            CameraPositionX = ObserverX(AngleX, AngleY);
            CameraPositionY = ObserverY(AngleY);
            CameraPositionZ = ObserverZ(AngleX, AngleY);

            Camera.Position = new Point3D(CameraPositionX, CameraPositionY, CameraPositionZ);
            Camera.LookDirection = new Vector3D(-CameraPositionX, AxisY - CameraPositionY, -CameraPositionZ);
            
            CopyCamera?.Invoke(Camera);
        }


        public double ObserverX(double azimuth, double altitude)
        {
            return CameraR * Math.Cos(azimuth) * Math.Cos(altitude);
        }

        public double ObserverY(double altitude)
        {
            return CameraR * Math.Sin(altitude);
        }

        public double ObserverZ(double azimuth, double altitude)
        {
            return CameraR * Math.Sin(azimuth) * Math.Cos(altitude);
        }


        public void MouseLeftButtonDown(object obj)
        {
            MouseLeftButtonStatus = true;
        }
        public void MouseLeftButtonRelease(object obj)
        {
            MouseLeftButtonStatus = false;
        }

        public void MouseRightButtonDown(object obj)
        {
            MouseRightButtonStatus = true;
        }

        public void MouseRightButtonRelease(object obj)
        {
            MouseRightButtonStatus = false;
        }
    }
}
