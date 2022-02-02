using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using Xunit;
using Xunit.Abstractions;
using static LanguageExt.Prelude;

namespace language_ext.kata.tests;

public class EitherExercises
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EitherExercises(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetTheResultOfDivide()
    {
        // Divide x = 9 by y = 2
        var eitherResult = Divide(9, 2);
        var result = eitherResult.IfLeft(0);

        eitherResult.IsRight.Should().BeTrue();
        eitherResult.IsLeft.Should().BeFalse();
        result.Should().Be(4);
    }

    [Fact]
    public void MapTheResultOfDivide()
    {
        // Divide x = 9 by y = 2 and add z to the result
        var z = 3;
        var result = Divide(9, 2)
            .Map(a => a + z)
            .IfLeft(0);

        result.Should().Be(7);
    }

    [Fact]
    public void DivideByZeroIsAlwaysAGoodIdea()
    {
        // Divide x by 0 and get the result
        var call = () => Divide(1, 0);
        var result = call.Invoke();

        result.IsLeft.Should().BeTrue();
        result.LeftUnsafe().Message.Should().Be("Dude, can't divide by 0");
    }

    [Fact]
    public void DivideByZeroOrElse()
    {
        // Divide x by 0, on exception returns 0
        var x = 1;
        var result = Divide(x, 0).IfLeft(0);

        result.Should().Be(0);
    }

    [Fact]
    public void MapTheFailure()
    {
        // Divide x by 0, log the failure message to the console and get 0
        var x = 1;
        var result = Divide(x, 0)
            .IfLeft(left =>
            {
                _testOutputHelper.WriteLine(left.Message);
                return 0;
            });

        result.Should().Be(0);
    }

    [Fact]
    public void ChainTheEither()
    {
        // Divide x by y
        // Chain 2 other calls to divide with x = previous Divide result
        // log the failure message to the console
        // Log your success to the console
        // Get the result or 0 if exception
        var x = 27;
        var y = 3;

        var result = Divide(x, y)
            .Bind(previous => Divide(previous, y))
            .Bind(previous => Divide(previous, y))
            .Do(success => _testOutputHelper.WriteLine($"Result is {success}"))
            .IfLeft(left =>
            {
                _testOutputHelper.WriteLine(left.Message);
                return 0;
            });

        result.Should().Be(1);
    }

    private static Either<Error, int> Divide(int x, int y)
        => y == 0 ? Left(Error.New("Dude, can't divide by 0")) : Right(x / y);
}
