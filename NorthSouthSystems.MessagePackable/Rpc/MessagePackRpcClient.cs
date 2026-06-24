using PolyType;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NorthSouthSystems.MessagePackable;

public class MessagePackRpcClient(HttpClient client)
{
    internal async Task<TResponse> ExecuteAsync<TRpc, TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken)
        where TRpc : MessagePackRpc<TRpc, TRequest, TResponse>
        where TRequest : IShapeable<TRequest>
        where TResponse : IShapeable<TResponse>
    {
        Throw.IfNull(client);

        var contentType = new MediaTypeWithQualityHeaderValue(MessagePackRpc.ContentType);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, MessagePackRpcHelpers.GetSubPath(typeof(TRpc)));
        requestMessage.Headers.Accept.Clear();
        requestMessage.Headers.Accept.Add(contentType);

        using var requestContent = new SerializerContent<TRequest>(request);
        requestMessage.Content = requestContent;
        requestContent.Headers.ContentType = contentType;

        using var responseMessage = await client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        responseMessage.EnsureSuccessStatusCode();

        var responseStream = await responseMessage.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        await using (responseStream.ConfigureAwait(false))
        {
            var response = await MessagePackRpc.GetMessagePackSerializer<TResponse>()
                .DeserializeAsync<TResponse>(responseStream, cancellationToken)
                .ConfigureAwait(false);

            return response!;
        }
    }

    private class SerializerContent<T>(T t) : HttpContent where T : IShapeable<T>
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) =>
            SerializeToStreamAsync(stream, context, CancellationToken.None);

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken) =>
            await MessagePackRpc.GetMessagePackSerializer<T>()
                .SerializeAsync(stream, t, cancellationToken)
                .ConfigureAwait(false);

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}