using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace language_ext.kata.tests;

public class EitherExercises
{
    private static Either<Error, int> Divide(int x, int y)
        => y == 0 ?
            Left(Error.New("Dude, can't divide by 0")) :
            Right(x / y);
}
