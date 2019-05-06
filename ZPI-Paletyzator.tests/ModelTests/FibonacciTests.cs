using NUnit.Framework;
using ZPI_Paletyzator.Model;
namespace ModelTests
{
    public class FibonacciTests
    {
        private Fibonacci fib = new Fibonacci();
        private double prevExpected = 2;
        private double nextExpected = 3;
        [SetUp]
        public void Setup()
        {
            fib.fibNum = 2;
            fib.fibNumPrev = 1;
        }

        [Test]
        public void isPrevNumCalculated()
        {
            fib.calculateNext();
            Assert.AreEqual(prevExpected, fib.fibNumPrev);
        }
        [Test]
        public void isNextNumNotTheSame()
        {
            fib.calculateNext();
            Assert.AreNotEqual(prevExpected, fib.fibNum);
        }
        [Test]
        public void isNextNumCalculated()
        {
            fib.calculateNext();
            Assert.AreEqual(nextExpected, fib.fibNum);
        }
    }
}