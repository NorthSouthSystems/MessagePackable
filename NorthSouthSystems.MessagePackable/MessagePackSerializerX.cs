using Nerdbank.MessagePack;
using PolyType;
using System.Text.Json.Nodes;

namespace NorthSouthSystems.MessagePackable;

public static class MessagePackSerializerX
{
    // GetJsonSchema does not support PreserveReferences.
    public static JsonObject GetJsonSchemaNoReferences<T>(this MessagePackSerializer messagePackSerializer)
        where T : IShapeable<T>
    {
        ArgumentNullException.ThrowIfNull(messagePackSerializer);

        return (messagePackSerializer with { PreserveReferences = ReferencePreservationMode.Off }).GetJsonSchema<T>();
    }
}