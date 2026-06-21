using Nerdbank.MessagePack;
using PolyType;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace NorthSouthSystems.MessagePackable;

public interface IMessagePackable
{
    static abstract MessagePackSerializer MessagePack { get; }
}

public static class MessagePackableX
{
#pragma warning disable CA1000 // Static members on generic types is the desired design.
#pragma warning disable CA1034 // False positive; analyzer bug for C# 14.
    extension<T>(T) where T : IMessagePackable, IShapeable<T>
    {
        public static T? FromMessagePack(byte[] bytes, CancellationToken token = default) =>
            T.MessagePack.Deserialize<T>(bytes, token);

        public static T? FromMessagePack(ImmutableArray<byte> bytes, CancellationToken token = default) =>
            T.MessagePack.Deserialize<T>(bytes.AsMemory(), token);

        public static T? ReadMessagePack(Stream stream, CancellationToken token = default) =>
            T.MessagePack.Deserialize<T>(stream, token);
    }

    extension<T>(T t) where T : IMessagePackable, IShapeable<T>
    {
        public ImmutableArray<byte> ToMessagePack(CancellationToken token = default) =>
            ImmutableCollectionsMarshal.AsImmutableArray(T.MessagePack.Serialize(t, token));

        public MessagePacked<T> ToMessagePacked(CancellationToken token = default) =>
            new(t.ToMessagePack(token));

        public void WriteMessagePack(Stream stream, CancellationToken token = default) =>
            T.MessagePack.Serialize(stream, t, token);
    }
#pragma warning restore
}