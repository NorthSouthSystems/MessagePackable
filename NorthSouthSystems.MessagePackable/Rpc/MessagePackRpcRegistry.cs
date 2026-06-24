using System.Reflection;

namespace NorthSouthSystems.MessagePackable;

public sealed record MessagePackRpcRegistration(
    Type RpcType, string SubPath, Type RequestType, Type ResponseType, ImmutableArray<string> Roles);

public sealed class MessagePackRpcRegistry(params IEnumerable<Assembly> assemblies)
{
    public ImmutableArray<MessagePackRpcRegistration> All { get; } =
    [
        .. (assemblies ?? [])
        .Distinct()
        .SelectMany(a => a.GetTypes())
        .Where(MessagePackRpcHelpers.IsMessagePackRpc)
        .SelectMany(MessagePackRpcHelpers.GetRpcTypes)
        .Select(t =>
        {
            var genericArgs = t.GetSubTypeOfGeneric(typeof(MessagePackRpc<,,>))!.GetGenericArguments();
            var roles = t.GetCustomAttribute<MessagePackRpcAuthorizeAttribute>()?.Roles;

            return new MessagePackRpcRegistration(
                t,
                MessagePackRpcHelpers.GetSubPath(t),
                genericArgs[1],
                genericArgs[2],
                roles ?? []
            );
        })
    ];
}