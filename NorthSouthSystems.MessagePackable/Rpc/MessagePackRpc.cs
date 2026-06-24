using Nerdbank.MessagePack;
using PolyType;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace NorthSouthSystems.MessagePackable;

public static class MessagePackRpc
{
    public const string ContentType = "application/x-msgpack";

    public static MessagePackSerializer GetMessagePackSerializer<T>()
        where T : IShapeable<T> =>
        typeof(IMessagePackable).IsAssignableFrom(typeof(T))
            ? (MessagePackSerializer)GetIMessagePackableMessagePackMethod
                .MakeGenericMethod(typeof(T))
                .Invoke(null, [])!
            : DefaultMessagePack;

    private static readonly MethodInfo GetIMessagePackableMessagePackMethod =
        typeof(MessagePackRpc)
            .GetMethod(nameof(GetIMessagePackableMessagePack), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static MessagePackSerializer GetIMessagePackableMessagePack<T>()
        where T : IMessagePackable =>
        T.MessagePack;

    private static readonly MessagePackSerializer DefaultMessagePack = new()
    {
        PreserveReferences = ReferencePreservationMode.RejectCycles
    };
}

#pragma warning disable CA1000 // In this case, the only public members are static, so call site ambiguity is unlikely.
public abstract class MessagePackRpc<TSelf, TRequest, TResponse>
    where TSelf : MessagePackRpc<TSelf, TRequest, TResponse>
    where TRequest : IShapeable<TRequest>
    where TResponse : IShapeable<TResponse>
{
    public static Task<TResponse> ExecuteRemotelyAsync(MessagePackRpcClient client,
        TRequest request, CancellationToken cancellationToken) =>
        Throw.IfNull(client).ExecuteAsync<TSelf, TRequest, TResponse>(request, cancellationToken);

    public async Task ExecuteLocallyAsync(Stream requestStream, Stream responseStream, CancellationToken cancellationToken)
    {
        Throw.IfNull(requestStream);
        Throw.IfNull(responseStream);

        var request = await MessagePackRpc.GetMessagePackSerializer<TRequest>()
            .DeserializeAsync<TRequest>(requestStream, cancellationToken)
            .ConfigureAwait(false);

        var response = await ExecuteLocallyAsync(request!, cancellationToken).ConfigureAwait(false);

        await MessagePackRpc.GetMessagePackSerializer<TResponse>()
            .SerializeAsync(responseStream, response, cancellationToken)
            .ConfigureAwait(false);
    }

    public abstract Task<TResponse> ExecuteLocallyAsync(TRequest request, CancellationToken cancellationToken);
}
#pragma warning restore

internal static class MessagePackRpcHelpers
{
    internal static bool IsMessagePackRpc(Type type) =>
        !Throw.IfNull(type).IsAbstract && type.IsSubTypeOfGeneric(typeof(MessagePackRpc<,,>));

    internal static string GetSubPath(Type type) => SubPathsByType.GetOrAdd(type, CreateSubPath);

    private static readonly ConcurrentDictionary<Type, string> SubPathsByType = new();

#pragma warning disable CA1308 // Lowercase is more "web appropriate.
    private static string CreateSubPath(Type type)
    {
        if (!IsMessagePackRpc(type))
            throw new ArgumentOutOfRangeException(nameof(type), "Must be non-abstract MessagePackRpc subclass.");

        string ns = (type.Namespace ?? "nss").Replace('.', '-');

        string typeName = SubPathTypeNameHelper(type)
            .Where(char.IsLetterOrDigit)
            .DelimitCamelCase("-")
            .ToNewString();

        return string.Create(InvariantCulture, $"{ns}/{typeName}").ToLowerInvariant();
    }
#pragma warning restore CA1308

    private static string SubPathTypeNameHelper(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        // Type.Name for a closed generic type is the same as its open name: simple name + backtick + generic arity.
        return type.IsConstructedGenericType
            ? string.Concat(type.GetGenericArguments().Select(SubPathTypeNameHelper).Prepend(type.Name.Split('`')[0]))
            : throw new ArgumentException("Type must not have any open generic type parameters.", nameof(type));
    }

    internal static IEnumerable<Type> GetRpcTypes(Type type)
    {
        var constructedGenericAttribute = type.GetCustomAttribute<MessagePackRpcConstructedGenericAttribute>();

        if (!type.IsGenericType || type.IsConstructedGenericType)
        {
            return constructedGenericAttribute is null
                ? [type]
                : throw new ArgumentException(string.Create(InvariantCulture, $"Type should not be decorated with {nameof(MessagePackRpcConstructedGenericAttribute)}."));
        }

        if (constructedGenericAttribute is null)
            throw new ArgumentException(string.Create(InvariantCulture, $"Type must be decorated with {nameof(MessagePackRpcConstructedGenericAttribute)}."));

        ArgumentExceptionX.ThrowIfAny(
            constructedGenericAttribute.ConstructedGenericTypes.Where(cgt => !cgt.IsSubTypeOfGeneric(type)));

        return constructedGenericAttribute.ConstructedGenericTypes;
    }
}