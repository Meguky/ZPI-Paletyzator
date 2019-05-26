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
        private double _palleteWidth;
        private double _palleteLength;
        private double _palleteMaxWeight;
        private double _palleteMaxHeight;
        private double _calculateOutput;
        private bool UsingEuroPalette { get; set; }

        public int a = 1;

        public ViewPortData ViewPortDataSource { get; private set; }

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
            optimization.packageHeight = _packageHeight;
            optimization.packageWidth = _packageWidth;
            optimization.packageLength = _packageLength;
            optimization.packageWeight = _packageWeight;
            optimization.seamFacingFront = _seamFacingFront;

            optimization.palleteWidth = _palleteWidth;
            optimization.palleteLength = _palleteLength;
            optimization.palleteMaxWeight = _palleteMaxWeight;
            optimization.palleteMaxHeight = _palleteMaxHeight;
            CalculateOutput = optimization.Calculate();
        }


        private bool CanCalculate(object commandParameter)
        {
            if (PackageHeight != 0 && PackageWidth != 0 && PackageLength != 0 && PackageWeight != 0 && PalleteWidth != 0 && PalleteLength != 0 && PalleteMaxWeight != 0 && PalleteMaxHeight != 0)
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
                PalleteWidth = 800;
                PalleteLength = 1200;
                PalleteMaxHeight = 2000;
                PalleteMaxWeight = 1500;
            }
            else
                PalleteWidth = PalleteLength = PalleteMaxHeight = PalleteMaxWeight = 0;
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
        public double PalleteWidth
        {
            get => _palleteWidth;
            set
            {
                SetProperty(ref _palleteWidth, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PalleteLength
        {
            get => _palleteLength;
            set
            {
                SetProperty(ref _palleteLength, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PalleteMaxWeight
        {
            get => _palleteMaxWeight;
            set
            {
                SetProperty(ref _palleteMaxWeight, value);
                _calculateCommand.InvokeCanExecuteChanged();
            }
        }
        public double PalleteMaxHeight
        {
            get => _palleteMaxHeight;
            set
            {
                SetProperty(ref _palleteMaxHeight, value);
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

