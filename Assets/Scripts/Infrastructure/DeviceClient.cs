using Securiton.Protocol;
using Securiton.Requests;
using Securiton.Serialization;
using Securiton.Transport;

namespace Securiton.Infrastructure
{
  public sealed class DeviceClient
  {
    private readonly ITransport _transport;
    private readonly IEncryptor _encryptor;
    private readonly IRequestPacketBuilder _packetBuilder;
    private readonly IResponsePacketParser _packetParser;

    public DeviceClient(
      ITransport transport,
      IEncryptor encryptor,
      IRequestPacketBuilder packetBuilder,
      IResponsePacketParser packetParser)
    {
      _transport = transport;
      _encryptor = encryptor;
      _packetBuilder = packetBuilder;
      _packetParser = packetParser;
    }

    public TResponse Send<TRequest, TResponse>(
      TRequest request,
      IRequestSerializer<TRequest> serializer,
      IResponseDeserializer<TResponse> deserializer)
      where TRequest : IDeviceRequest<TResponse>
    {
      byte[] payload = serializer.Serialize(request);
      byte[] packet = _packetBuilder.Build(request.RequestId, payload);
      byte[] encryptedRequest = _encryptor.Encrypt(packet);

      byte[] encryptedResponse = _transport.SendAndReceive(encryptedRequest);
      byte[] responseBytes = _encryptor.Decrypt(encryptedResponse);

      ResponsePacket responsePacket = _packetParser.Parse(responseBytes);

      return deserializer.Deserialize(responsePacket.Payload);
    }
  }
}