using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZPI_Paletyzator.Helper;
using ZPI_Paletyzator.Model;

namespace ZPI_Paletyzator.ViewModel
{
    public class FibonacciViewModel : ViewModelBase
    {
        private Fibonacci _fib = new Fibonacci();
        private double _fibNum;
        private double _fibNumPrev;
        private readonly DelegateCommand _changeFibNumCommand;
        public ICommand ChangeFibNumCommand => _changeFibNumCommand;

        public FibonacciViewModel()
        {
            _changeFibNumCommand = new DelegateCommand(OnChangeFibNum, CanChangeName);
            FibNum = _fib.fibNum;
            FibNumPrev = _fib.fibNumPrev;
        }

        private void OnChangeFibNum(object commandParameter)
        {
            _fib.calculateNext();
            FibNum = _fib.fibNum;
            FibNumPrev = _fib.fibNumPrev;
            _changeFibNumCommand.InvokeCanExecuteChanged();
        }
        
        private bool CanChangeName(object commandParameter)
        {
            return FibNum < 10000000;
        }

        public double FibNum{
            get => _fibNum;
            set => SetProperty(ref _fibNum, value);
        }
        public double FibNumPrev
        {
            get => _fibNumPrev;
            set => SetProperty(ref _fibNumPrev, value);
        }


    }
}
