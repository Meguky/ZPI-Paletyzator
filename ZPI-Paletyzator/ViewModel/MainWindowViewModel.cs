using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using ZPI_Paletyzator.Helper;
using ZPI_Paletyzator.Model;
using ZPI_Paletyzator.View;


namespace ZPI_Paletyzator.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private OptimizationMain optimization = new OptimizationMain();
        private readonly DelegateCommand _calculateCommand;
        private readonly DelegateCommand _seamFacingFrontCommand;
        private readonly DelegateCommand _euroPaletteCommand;
        private double _packageHeight;
        private double _packageWidth;
        private double _packageLength;
        private double _packageWeight;
        private bool _seamFacingFront;
        private double _paletteWidth;
        private double _paletteLength;
        private double _paletteMaxWeight;
        private double _paletteMaxHeight;
        private double _calculateOutput;
        private bool UsingEuroPalette { get; set; }

        public ViewPortData ViewPortDataSource { get; set; }

        public ICommand CalculateCommand => _calculateCommand;
        public ICommand SeamFacingFrontCommand => _seamFacingFrontCommand;
        public ICommand EuroPaletteCommand => _euroPaletteCommand;

        public MainWindowViewModel()
        {
            _calculateCommand = new DelegateCommand(Calculate, CanCalculate);
            _seamFacingFrontCommand = new DelegateCommand(ChangeSeamPosition);
            _euroPaletteCommand = new DelegateCommand(EuroPalette);
            _seamFacingFront = false;
            ViewPortDataSource = new ViewPortData();
        }
       

        private void Calculate(object commandParameter)
        {
            ViewPortDataSource.AddSceneObjects(PackageHeight, PackageWidth, PackageLength, PaletteWidth, PaletteLength);
            RaiseSetPropertyChangedEvent();
        }


        private bool CanCalculate(object commandParameter)
        {
            if (PackageHeight != 0 && PackageWidth != 0 && PackageLength != 0 && PackageWeight != 0 && PaletteWidth != 0 && PaletteLength != 0 && PaletteMaxWeight != 0 && PaletteMaxHeight != 0)
                return true;
            else
                return false;
        }

        private void ChangeSeamPosition(object commandParameter)
        {
            _seamFacingFront = !_seamFacingFront;
        }

        private void EuroPalette(object commandParameter)
        {
            UsingEuroPalette = ! UsingEuroPalette;
            if (UsingEuroPalette)
            {
                PaletteWidth = 800;
                PaletteLength = 1200;
                PaletteMaxHeight = 2000;
                PaletteMaxWeight = 1500;
            }
            else
                PaletteWidth = PaletteLength = PaletteMaxHeight = PaletteMaxWeight = 0;
        }

        public double PackageHeight
        {
            get => _packageHeight;
            set
            {
                SetProperty(ref _packageHeight, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }

        public double PackageWidth
        {
            get => _packageWidth;
            set
            {
                SetProperty(ref _packageWidth, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }

        public double PackageLength
        {
            get => _packageLength;
            set
            {
                SetProperty(ref _packageLength, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PackageWeight
        {
            get => _packageWeight;
            set
            {
                SetProperty(ref _packageWeight, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PaletteWidth
        {
            get => _paletteWidth;
            set
            {
                SetProperty(ref _paletteWidth, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PaletteLength
        {
            get => _paletteLength;
            set
            {
                SetProperty(ref _paletteLength, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PaletteMaxWeight
        {
            get => _paletteMaxWeight;
            set
            {
                SetProperty(ref _paletteMaxWeight, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PaletteMaxHeight
        {
            get => _paletteMaxHeight;
            set
            {
                SetProperty(ref _paletteMaxHeight, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double CalculateOutput
        {
            get => _calculateOutput;
            set => SetProperty(ref _calculateOutput, value);
        }
    }
}

