// T4-generated @ 2026-06-24 03:05:50 UTC
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace NorthSouthSystems.MessagePackable;

[ExcludeFromCodeCoverage]
internal static class ArgumentExceptionX
{
    public static void ThrowIfAny<T>(IEnumerable<T>? enumerable,
        string? messagePrefix = null, bool messageIncludeIndices = false,
        string? originalParamName = null, [CallerArgumentExpression(nameof(enumerable))] string? paramName = null)
    {
        if (enumerable is null)
            return;

        StringBuilder? message = null;
        int index = 0;

        foreach (var t in enumerable)
        {
            if (message is null)
            {
                message = new(messagePrefix);

                if (!string.IsNullOrEmpty(messagePrefix) && !messagePrefix.EndsWith('\n'))
                    message.AppendLine();
            }
            else
                message.AppendLine();

            if (messageIncludeIndices)
            {
                message.Append(index++.ToString(InvariantCulture));
                message.Append(": ");
            }

            message.Append(t?.ToString());
        }

        if (message is null)
            return;

        throw new ArgumentException(message.ToString(), originalParamName ?? paramName);
    }

    public static void ThrowIfDefault<T>([NotNull] T? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct
    {
        if (argument is null)
            throw new ArgumentNullException(paramName, "Value cannot be null.");

        ThrowIfDefault(argument.Value, paramName);
    }

    public static void ThrowIfDefault<T>(T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct
    {
        if (argument.Equals(default(T)))
            throw new ArgumentException("Value cannot be default.", paramName);
    }
}

#pragma warning disable CA1716 // Accepted as the preferred design.
[ExcludeFromCodeCoverage]
internal static class Throw
#pragma warning restore
{
    public static T IfNull<T>([NotNull] T? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(argument, paramName);

        return argument;
    }

    public static T IfDefault<T>(T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct
    {
        ArgumentExceptionX.ThrowIfDefault(argument, paramName);

        return argument;
    }

    public static T IfDefault<T>([NotNull] T? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : struct
    {
        ArgumentExceptionX.ThrowIfDefault(argument, paramName);

        return argument.Value;
    }

    #region String (ArgumentException pass-through)

    public static string IfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);

        return argument;
    }

    public static string IfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);

        return argument;
    }

    #endregion

    #region INumberBase<T> (ArgumentOutOfRangeException pass-through)

    public static T IfZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfZero(value, paramName);

        return value;
    }

    public static T IfNegative<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value, paramName);

        return value;
    }

    public static T IfNegativeOrZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, paramName);

        return value;
    }

    #endregion

    #region Equality (ArgumentOutOfRangeException pass-through)

    public static T IfEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, other, paramName);

        return value;
    }

    public static T IfNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(value, other, paramName);

        return value;
    }

    #endregion

    #region IComparable<T> (ArgumentOutOfRangeException pass-through)

    public static T IfGreaterThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other, paramName);

        return value;
    }

    public static T IfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other, paramName);

        return value;
    }

    public static T IfLessThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, other, paramName);

        return value;
    }

    public static T IfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other, paramName);

        return value;
    }

    #endregion

    #region Comparer<TEnum> (custom)

    public static TEnum IfGreaterThanEnum<TEnum>(TEnum value, TEnum other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        if (Comparer<TEnum>.Default.Compare(value, other) > 0)
            ComparerTEnumThrow(value, other, paramName, "less than or equal to");

        return value;
    }

    public static TEnum IfGreaterThanOrEqualEnum<TEnum>(TEnum value, TEnum other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        if (Comparer<TEnum>.Default.Compare(value, other) >= 0)
            ComparerTEnumThrow(value, other, paramName, "less than");

        return value;
    }

    public static TEnum IfLessThanEnum<TEnum>(TEnum value, TEnum other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        if (Comparer<TEnum>.Default.Compare(value, other) < 0)
            ComparerTEnumThrow(value, other, paramName, "greater than or equal to");

        return value;
    }

    public static TEnum IfLessThanOrEqualEnum<TEnum>(TEnum value, TEnum other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        if (Comparer<TEnum>.Default.Compare(value, other) <= 0)
            ComparerTEnumThrow(value, other, paramName, "greater than");

        return value;
    }

    // Extracted in order to make the successful path smaller and more likely to inline.
    [DoesNotReturn]
    private static void ComparerTEnumThrow<TEnum>(TEnum value, TEnum other, string? paramName, string inverseOperatorFriendlyName)
        where TEnum : struct, Enum =>
        throw new ArgumentOutOfRangeException(paramName, value,
            string.Create(InvariantCulture, $"{paramName} ('{value}') must be {inverseOperatorFriendlyName} '{other}'."));

    #endregion

    #region IComparable<T> Between (custom)

    public static T IfBetween<T>(T value, T leftInclusive, T rightInclusive, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T> =>
        BetweenHelper(value, leftInclusive, rightInclusive, paramName, true);

    public static T IfNotBetween<T>(T value, T leftInclusive, T rightInclusive, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T> =>
        BetweenHelper(value, leftInclusive, rightInclusive, paramName, false);

    private static T BetweenHelper<T>(T value, T leftInclusive, T rightInclusive, string? paramName, bool isBetweenThrows)
        where T : IComparable<T>
    {
        // The ArgumentOutOfRangeException.Throw* methods do NOT validate their parameters and allow
        // NullReferenceExceptions to occur accordingly.

        if (leftInclusive.CompareTo(rightInclusive) > 0)
            (leftInclusive, rightInclusive) = (rightInclusive, leftInclusive);

        bool isBetween = value.CompareTo(leftInclusive) >= 0 && value.CompareTo(rightInclusive) <= 0;

        if (isBetween == isBetweenThrows)
            BetweenHelperThrow(value, leftInclusive, rightInclusive, paramName, isBetweenThrows);

        return value;
    }

    // Extracted in order to make the successful path smaller and more likely to inline.
    [DoesNotReturn]
    private static void BetweenHelperThrow<T>(T value, T leftInclusive, T rightInclusive, string? paramName, bool isBetweenThrows)
        where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(paramName, value,
            string.Create(InvariantCulture, $"{paramName} ('{value}') must {(isBetweenThrows ? "not " : string.Empty)}be between '{leftInclusive}' and '{rightInclusive}' inclusively."));

    #endregion

    #region Comparer<TEnum> Between (custom)

    public static TEnum IfBetweenEnum<TEnum>(TEnum value, TEnum leftInclusive, TEnum rightInclusive, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum =>
        BetweenEnumHelper(value, leftInclusive, rightInclusive, paramName, true);

    public static TEnum IfNotBetweenEnum<TEnum>(TEnum value, TEnum leftInclusive, TEnum rightInclusive, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum =>
        BetweenEnumHelper(value, leftInclusive, rightInclusive, paramName, false);

    private static TEnum BetweenEnumHelper<TEnum>(TEnum value, TEnum leftInclusive, TEnum rightInclusive, string? paramName, bool isBetweenThrows)
        where TEnum : struct, Enum
    {
        var comparer = Comparer<TEnum>.Default;

        if (comparer.Compare(leftInclusive, rightInclusive) > 0)
            (leftInclusive, rightInclusive) = (rightInclusive, leftInclusive);

        bool isBetween = comparer.Compare(value, leftInclusive) >= 0 && comparer.Compare(value, rightInclusive) <= 0;

        if (isBetween == isBetweenThrows)
            BetweenEnumHelperThrow(value, leftInclusive, rightInclusive, paramName, isBetweenThrows);

        return value;
    }

    // Extracted in order to make the successful path smaller and more likely to inline.
    [DoesNotReturn]
    private static void BetweenEnumHelperThrow<TEnum>(TEnum value, TEnum leftInclusive, TEnum rightInclusive, string? paramName, bool isBetweenThrows)
        where TEnum : struct, Enum =>
        throw new ArgumentOutOfRangeException(paramName, value,
            string.Create(InvariantCulture, $"{paramName} ('{value}') must {(isBetweenThrows ? "not " : string.Empty)}be between '{leftInclusive}' and '{rightInclusive}' inclusively."));

    #endregion
}

[ExcludeFromCodeCoverage]
internal static class TypeX
{
    public static object? Default(this Type type) =>
        Throw.IfNull(type).IsValueType
            ? RuntimeHelpers.GetUninitializedObject(type)
            : null;

    // Unfortunately, there is no simpler method to determine this. All Systems.Numerics interfaces
    // are recursive generics (i.e. IInterface<T> where T : IInterface<T>), so they can't be used
    // with "is" or "as" operators on instances or with Type.IsAssignable for Types (without Reflection).
    public static bool IsFloatingPoint(this Type type) =>
        FloatingPointTypes.Contains(Throw.IfNull(type));

    public static ImmutableHashSet<Type> FloatingPointTypes { get; } =
    [
        typeof(Half),
        typeof(float),
        typeof(double),
        typeof(decimal)
    ];

    // Unfortunately, there is no simpler method to determine this. All Systems.Numerics interfaces
    // are recursive generics (i.e. IInterface<T> where T : IInterface<T>), so they can't be used
    // with "is" or "as" operators on instances or with Type.IsAssignable for Types (without Reflection).
    public static bool IsIntegral(this Type type) =>
        IntegralTypes.Contains(Throw.IfNull(type));

    public static ImmutableHashSet<Type> IntegralTypes { get; } =
    [
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),

        typeof(Int128),
        typeof(BigInteger)
    ];

    internal static bool CanBeEnumUnderlyingType(this Type type) =>
        EnumUnderlyingTypes.Contains(Throw.IfNull(type));

    public static ImmutableHashSet<Type> EnumUnderlyingTypes { get; } =
    [
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong)
    ];

    public static ImmutableDictionary<Type, string> CSharpKeywordsByType { get; } =
        new Dictionary<Type, string>
            {
                [typeof(bool)] = "bool",
                [typeof(byte)] = "byte",
                [typeof(sbyte)] = "sbyte",
                [typeof(short)] = "short",
                [typeof(ushort)] = "ushort",
                [typeof(int)] = "int",
                [typeof(uint)] = "uint",
                [typeof(long)] = "long",
                [typeof(ulong)] = "ulong",
                [typeof(float)] = "float",
                [typeof(double)] = "double",
                [typeof(decimal)] = "decimal",

                [typeof(char)] = "char",
                [typeof(string)] = "string",

                [typeof(object)] = "object"
            }
            .ToImmutableDictionary();

    public static bool IsGenericNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;
    public static Type FlattenGenericNullable(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

    public static bool IsSubTypeOfGeneric(this Type type, Type genericTypeDefinition) =>
        GetSubTypeOfGeneric(type, genericTypeDefinition) is not null;

    public static Type? GetSubTypeOfGeneric(this Type type, Type genericTypeDefinition)
    {
        Throw.IfNull(type);

        if (!Throw.IfNull(genericTypeDefinition).IsGenericTypeDefinition)
            throw new ArgumentException("Must be a generic type definition.", nameof(genericTypeDefinition));

        var types = genericTypeDefinition.IsInterface
            ? type.GetInterfaces()
            : SelfAndBaseTypes(type);

        return types.SingleOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericTypeDefinition);
    }

    public static IEnumerable<Type> SelfAndBaseTypes(Type? t)
    {
        while (true)
        {
            if (t is null)
                yield break;

            yield return t;

            t = t.BaseType;
        }
    }
}
