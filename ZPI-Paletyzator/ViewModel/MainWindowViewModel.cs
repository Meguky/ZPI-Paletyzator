using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ViewPortInit ViewPortSource { get; private set; }

        public ICommand CalculateCommand => _calculateCommand;
        public ICommand SeamFacingFrontCommand => _seamFacingFrontCommand;
        public MainWindowViewModel()
        {
            _calculateCommand = new DelegateCommand(Calculate, CanCalculate);
            _seamFacingFrontCommand = new DelegateCommand(ChangeSeamPosition);
            _seamFacingFront = false;
            ViewPortSource = new ViewPortInit();
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
            return true;
        }

        private void ChangeSeamPosition(object commandParameter)
        {
            _seamFacingFront = !_seamFacingFront;
        }

        public double PackageHeight
        {
            get => _packageHeight;
            set => SetProperty(ref _packageHeight, value);
        }

        public double PackageWidth
        {
            get => _packageWidth;
            set => SetProperty(ref _packageWidth, value);
        }

        public double PackageLength
        {
            get => _packageLength;
            set => SetProperty(ref _packageLength, value);
        }
        public double PackageWeight
        {
            get => _packageWeight;
            set => SetProperty(ref _packageWeight, value);
        }
        public double PalleteWidth
        {
            get => _palleteWidth;
            set => SetProperty(ref _palleteWidth, value);
        }
        public double PalleteLength
        {
            get => _palleteLength;
            set => SetProperty(ref _palleteLength, value);
        }
        public double PalleteMaxWeight
        {
            get => _palleteMaxWeight;
            set => SetProperty(ref _palleteMaxWeight, value);
        }
        public double PalleteMaxHeight
        {
            get => _palleteMaxHeight;
            set => SetProperty(ref _palleteMaxHeight, value);
        }
        public double CalculateOutput
        {
            get => _calculateOutput;
            set => SetProperty(ref _calculateOutput, value);
        }
    }
}
