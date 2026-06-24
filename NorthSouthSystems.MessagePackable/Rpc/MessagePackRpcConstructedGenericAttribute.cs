namespace NorthSouthSystems.MessagePackable;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MessagePackRpcConstructedGenericAttribute : Attribute
{
#pragma warning disable CA1019 // Exposed as ImmutableArray property.
    public MessagePackRpcConstructedGenericAttribute(params Type[] constructedGenericTypes)
    {
        Throw.IfNull(constructedGenericTypes);
        Throw.IfZero(constructedGenericTypes.Length);
        ArgumentExceptionX.ThrowIfAny(constructedGenericTypes.Where(cgt => !cgt.IsConstructedGenericType));

        ConstructedGenericTypes = [.. constructedGenericTypes];
    }
#pragma warning restore

    public ImmutableArray<Type> ConstructedGenericTypes { get; }
}