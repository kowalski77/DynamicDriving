using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DynamicDriving.SharedKernel;

public static class Guards
{
    public static T ThrowIfNull<T>(T argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            ThrowNull(paramName);
        }

        return argument;
    }

    public static decimal ThrowIfLessThan(decimal argument, decimal value, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument <= value)
        {
            ThrowOutOfRange(paramName);
        }

        return argument;
    }

    public static Guid ThrowIfEmpty(Guid argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument == Guid.Empty)
        {
            ThrowGuidEmpty(paramName);
        }

        return argument;
    }

    public static string ThrowIfNullOrEmpty(string argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            ThrowGuidEmpty(paramName);
        }

        return argument;
    }

    [DoesNotReturn]
    private static void ThrowOutOfRange(string? paramName)
    {
        throw new ArgumentOutOfRangeException(paramName);
    }

    [DoesNotReturn]
    private static void ThrowNull(string? paramName)
    {
        throw new ArgumentNullException(paramName);
    }

    [DoesNotReturn]
    private static void ThrowGuidEmpty(string? paramName)
    {
        throw new ArgumentException("The guid has empty value", paramName);
    }
}
