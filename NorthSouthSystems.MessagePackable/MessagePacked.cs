using PolyType;
using System.IO.Hashing;

namespace NorthSouthSystems.MessagePackable;

public class MessagePacked<T> where T : IMessagePackable, IShapeable<T>
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

    public ImmutableArray<byte> Bytes { get; }
    public UInt128 XxHash128 { get; }

    public T ToT(CancellationToken token = default) =>
        T.MessagePack.Deserialize<T>(Bytes.AsMemory(), token)!;

    public void RoundTripThrowIfDiff(CancellationToken token = default)
    {
        var roundTrip = ToT(token).ToMessagePacked(token);

        if (XxHash128 != roundTrip.XxHash128)
            throw new ArgumentException("MessagePack round-trip failure.");
    }
}
