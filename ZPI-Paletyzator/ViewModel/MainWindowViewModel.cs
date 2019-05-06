using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZPI_Paletyzator.Helper;
using ZPI_Paletyzator.Model;

namespace ZPI_Paletyzator.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        OptimizationMain optimization = new OptimizationMain();
        private readonly DelegateCommand _calculateCommand;
        private readonly DelegateCommand _seamAtBeginCommand;
        private double _packageHeight;
        private double _packageWidth;
        private double _packageLength;
        private double _packageWeight;
        private bool _seamAtBegin;
        private double _palleteWidth;
        private double _palleteLength;
        private double _palleteMaxWeight;
        private double _palleteMaxHeight;
        private double _calculateOutput;
        public ICommand calculateCommand => _calculateCommand;
        public ICommand seamAtBeginCommand => _seamAtBeginCommand;
        public MainWindowViewModel()
        {
            _calculateCommand = new DelegateCommand(calculate, canCalculate);
            _seamAtBeginCommand = new DelegateCommand(changeSeamPosition);
            _seamAtBegin = false;
        }

        private void calculate(object commandParameter)
        {
            optimization.packageHeight = _packageHeight;
            optimization.packageWidth = _packageWidth;
            optimization.packageLength = _packageLength;
            optimization.packageWeight = _packageWeight;
            optimization.seamAtBegin = _seamAtBegin;

            optimization.palleteWidth = _palleteWidth;
            optimization.palleteLength = _palleteLength;
            optimization.palleteMaxWeight = _palleteMaxWeight;
            optimization.palleteMaxHeight = _palleteMaxHeight;
            CalculateOutput = optimization.calculate();
        }

        private bool canCalculate(object commandParameter)
        {
            return true;
        }

        private void changeSeamPosition(object commandParameter)
        {
            _seamAtBegin = !_seamAtBegin;
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
