using System;
using FluentAssertions;
using LanguageExt;
using Xunit;
using Xunit.Abstractions;
using static LanguageExt.Prelude;

namespace language_ext.kata.tests
{
    public class TryExercises : PetDomainKata
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TryExercises(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private const string SuccessMessage = "I m a fucking genius the result is ";

        [Fact]
        public void GetTheResultOfDivide()
        {
            // Divide x = 9 by y = 2
            var tryResult = Divide(9, 2);
            var result = tryResult.Match(x => x, 0);

            result.Should().Be(4);
            tryResult.IsSucc().Should().BeTrue();
            tryResult.IsDefault().Should().BeFalse();
            tryResult.IsFail().Should().BeFalse();
        }

        [Fact]
        public void MapTheResultOfDivide()
        {
            // Divide x = 9 by y = 2 and add z to the result
            const int z = 3;
            var result = Divide(9, 2)
                .Map(a => a + z)
                .Match(x => x, 0);

            result.Should().Be(7);
        }

        [Fact]
        public void DivideByZeroIsAlwaysAGoodIdea()
        {
            // Divide x by 0 and get the result
            const int x = 1;
            var divideByZero = () =>
                Divide(x, 0)
                    .Match(success => success, ex => throw ex);

            divideByZero.Should().Throw<DivideByZeroException>();
        }

        [Fact]
        public void DivideByZeroOrElse()
        {
            // Divide x by 0, on exception returns 0
            const int x = 1;
            var result = Divide(x, 0).IfFail(0);

            result.Should().Be(0);
        }

        [Fact]
        public void MapTheFailure()
        {
            // Divide x by 0, log the failure message to the console and get 0
            const int x = 1;
            var result = Divide(x, 0)
                .IfFail(failure =>
                {
                    _testOutputHelper.WriteLine(failure.Message);
                    return 0;
                });

            result.Should().Be(0);
        }

        [Fact]
        public void MapTheSuccess()
        {
            // Divide x by y
            // log the failure message to the console
            // Log your success to the console
            // Get the result or 0 if exception
            var x = 8;
            var y = 4;

            var result = Divide(x, y)
                // Can be written differently by using IfFail -> check #MapTheSuccessWithoutMatch
                .Match(success =>
                    {
                        _testOutputHelper.WriteLine(SuccessMessage + success);
                        return success;
                    },
                    failure =>
                    {
                        _testOutputHelper.WriteLine(failure.Message);
                        return 0;
                    });

            result.Should().Be(2);
        }

        [Fact]
        public void MapTheSuccessWithoutMatch()
        {
            // Divide x by y
            // log the failure message to the console
            // Log your success to the console
            // Get the result or 0 if exception
            const int x = 8;
            const int y = 4;

            var result = Divide(x, y)
                .Do(r => _testOutputHelper.WriteLine(SuccessMessage + r))
                .IfFail(failure =>
                {
                    _testOutputHelper.WriteLine(failure.Message);
                    return 0;
                });

            Assert.Equal(2, result);
        }

        [Fact]
        public void ChainTheTry()
        {
            // Divide x by y
            // Chain 2 other calls to divide with x = previous Divide result
            // log the failure message to the console
            // Log your success to the console
            // Get the result or 0 if exception
            const int x = 27;
            const int y = 3;

            var result = Divide(x, y)
                .Bind(previous => Divide(previous, y))
                .Bind(previous => Divide(previous, y))
                .Do(success => _testOutputHelper.WriteLine(SuccessMessage + success))
                .IfFail(failure =>
                {
                    _testOutputHelper.WriteLine(failure.Message);
                    return 0;
                });

            result.Should().Be(1);
        }

        private static Try<int> Divide(int x, int y)
            => Try(() => x / y);
    }
}
