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
        private readonly DelegateCommand _changeFibNum;
        public ICommand ChangeFibNumCommand => _changeFibNum;

        public FibonacciViewModel()
        {
            _changeFibNum = new DelegateCommand(OnChangeFibNum);
        }

        private void OnChangeFibNum(object commandParameter)
        {
            _fib.calculateNext();
        }
        
        public double FibNum{
            get => _fib.FibNum;
            set => SetProperty(ref _fib.FibNum, value);
        }
        public double FibNumPrev
        {
            get => _fib.FibNumPrev;
            set => SetProperty(ref _fib.FibNumPrev, value);
        }
    }
}
