using System;
using LanguageExt;

namespace language_ext.kata.tests;

public static class LanguageExtExtensions
{
    public static T GetUnsafe<T>(this Option<T> option)
        => option.IfNone(() => throw new ArgumentException("Can not Get from None"));
}
