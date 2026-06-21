using PolyType;
using System.Collections.Immutable;
using System.IO.Hashing;
using System.Threading;

namespace NorthSouthSystems.MessagePackable;

public class MessagePacked<T> : MessagePacked
    where T : IMessagePackable, IShapeable<T>
{
    public MessagePacked(ImmutableArray<byte> bytes) : base(bytes) { }
    public MessagePacked(ImmutableArray<byte> bytes, UInt128 xxHash128) : base(bytes, xxHash128) { }

    public T ToT(CancellationToken token = default) =>
        T.MessagePack.Deserialize<T>(Bytes.AsMemory(), token)!;

    public void RoundTripThrowIfDiff(CancellationToken token = default)
    {
        var roundTrip = ToT(token).ToMessagePacked(token);

        if (XxHash128 != roundTrip.XxHash128)
            throw new ArgumentException("MessagePack round-trip failure.");
    }
}

public class MessagePacked
{
    public MessagePacked(ImmutableArray<byte> bytes)
    {
        if (bytes.IsDefault)
            throw new ArgumentException("Cannot be default ImmutableArray.", nameof(bytes));

        Bytes = bytes;

        var hasher = new XxHash128();
        hasher.Append(Bytes.AsSpan());
        XxHash128 = hasher.GetCurrentHashAsUInt128();
    }

    public MessagePacked(ImmutableArray<byte> bytes, UInt128 xxHash128) : this(bytes) =>
        ArgumentOutOfRangeException.ThrowIfNotEqual(xxHash128, XxHash128);

    public ImmutableArray<byte> Bytes { get; }
    public UInt128 XxHash128 { get; }
}