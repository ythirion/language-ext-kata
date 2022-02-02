using System;
using FluentAssertions;
using LanguageExt;
using Xunit;

namespace language_ext.kata.tests
{
    public class PlayWithFuncExercises
    {
        private static readonly Func<int, int, int> Add = (x, y) => x + y;
        private static readonly Func<int, int, int> Multiply = (x, y) => x * y;
        private static readonly Func<int, int, int> Divide = (x, y) => x / y;

        private static readonly Func<int, int> Add1 = x => Add(1, x);
        private static readonly Func<int, int> Double = x => Multiply(x, 2);

        [Fact]
        public void Add1AndDoubleIt()
        {
            // Create an Add1 function based on Add function
            // Create a Double function based on Multiply
            // Compose the 2 functions together to implement the Add1AndDouble function
            int Add1AndDouble(int x) => Add1.Compose(Double)(x);
            Add1AndDouble(2).Should().Be(6);
        }

        [Fact]
        public void AddXtoYAndDoubleIt()
        {
            // Use the Double and the Add function to implement it
            int AddXtoYAndDouble(int x, int y) => Double(Add(x, y));
            AddXtoYAndDouble(2, 5).Should().Be(14);
        }

        private static readonly Func<int, int, Try<int>> TryMultiply = (x, y) => () => Multiply(x, y);
        private static readonly Func<int, int, Try<int>> TryDivide = (x, y) => () => Divide(x, y);

        [Fact]
        public void MultiplyXByYThemDivideByZSafely()
        {
            // Multiply x by y safely
            // Then Divide the result by z
            // Encapsulate each call inside a Try
            int MultiplyXByYThemDivideByZ(int x, int y, int z) =>
                TryMultiply(x, y)
                    .Bind(multiplied => TryDivide(multiplied, z))
                    .IfFail(0);

            MultiplyXByYThemDivideByZ(9, 3, 5).Should().Be(5);
        }

        private static int Formula(int x, int y, int z, int w)
            => Multiply(Add(Multiply(2, x), y), Divide(z, Multiply(w, 3)));

        [Fact]
        public void Formula1()
        {
            // Use the functions defined in this class to implement this formula
            // (2x + y) * (z / 3w)
            Formula(5, 4, 23, 2).Should().Be(42);
        }

        private static Try<int> SafeFormula(int x, int y, int z, int w) => ()
            => Formula(x, y, z, w);

        [Fact]
        public void FormulaE()
        {
            // Make the call to the previous formula Safe
            // By encapsulating the call in a Try monad
            SafeFormula(5, 4, 23, 0).IsFail().Should().BeTrue();
        }
    }
}
