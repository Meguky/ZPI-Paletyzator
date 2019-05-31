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
        //private double AxisY { get; set; }
        //private double AxisOffsetY { get; set; }
        
        private PerspectiveCamera Camera { get; set; }

        private Action<PerspectiveCamera> CopyCamera;

        private double OldPanelWidth { get; set; }
        private double originalWidth = 0;
        private double originalHeight = 0;

        

        public MouseControlCamera (Action<PerspectiveCamera> changeMainCamera)
        {
            CameraR = 25;
            //AxisY = 2;
            Camera = new PerspectiveCamera()
            {
                FieldOfView = 45
            };
            CopyCamera = changeMainCamera;
            SetCamera();
        }



        public void InitPanel(object obj)
        {
            if (obj is Panel PanelObject)
            {
                Pix2AngleX = 360 * 0.01 / PanelObject.ActualWidth;
                Pix2AngleY = 360 * 0.01 / PanelObject.ActualHeight;
                originalWidth = OldPanelWidth = PanelObject.ActualWidth;
                originalHeight = PanelObject.ActualHeight;
            }
        }



        public void GetPanelSize(object obj)
        {
            if (obj is Panel PanelObject)
            {
                double newWidth = PanelObject.ActualWidth;
                double originalNearPlaneDistance = 0.125;
                double originalFieldOfView = 45.0;
                double scale = newWidth / originalWidth;

                double fov = Math.Atan(Math.Tan(originalFieldOfView / 2.0 / 180.0 * Math.PI) * scale) * 2.0;
                Camera.FieldOfView = fov / Math.PI * 180.0;
                Camera.NearPlaneDistance = originalNearPlaneDistance * scale;

                //double heightRatio = PanelObject.ActualHeight / originalHeight;
                //AxisY = 2;
                //AxisY *= Math.Pow(heightRatio,2);
                //AxisY += AxisOffsetY;


                SetCamera();
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
                    else if (AngleY > Math.PI / 3)
                        AngleY = Math.PI / 3;

                }


                if (MouseRightButtonStatus)
                {
                    double newCameraR = CameraR + (deltaY * Pix2AngleY) * 10;

                    if (newCameraR >= 10 && newCameraR < 50)
                    {
                        CameraR = newCameraR;
                        //AxisY += deltaY * Pix2AngleY * 4;
                        //AxisOffsetY += deltaY * Pix2AngleY * 4;
                    }
                }

                if (MouseLeftButtonStatus || MouseRightButtonStatus)
                {
                    SetCamera();
                }
            }
        }



        private void SetCamera()
        {
            CameraPositionX = ObserverX(AngleX, AngleY);
            CameraPositionY = ObserverY(AngleY);
            CameraPositionZ = ObserverZ(AngleX, AngleY);

            Camera.Position = new Point3D(CameraPositionX, CameraPositionY, CameraPositionZ);
            Camera.LookDirection = new Vector3D(-CameraPositionX, /*AxisY*/ - CameraPositionY, -CameraPositionZ);
            
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
