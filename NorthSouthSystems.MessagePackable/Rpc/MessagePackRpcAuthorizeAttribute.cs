namespace NorthSouthSystems.MessagePackable;

[AttributeUsage(AttributeTargets.Class)]
#pragma warning disable CA1019 // ImmutableArray version of the propery exists.
public sealed class MessagePackRpcAuthorizeAttribute(params string[] roles) : Attribute
{
    public ImmutableArray<string> Roles { get; } =
        roles is not null
            ? [.. roles.Select(r => r.Trim()).Where(string.IsNotNullAndNotWhiteSpace)]
            : [];
}
#pragma warning restore