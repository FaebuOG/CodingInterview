namespace Securiton.Protocol
{
  public sealed class ResponsePacket
  {
    public byte RequestId { get; }
    public byte StatusCode { get; }
    public byte[] Payload { get; }

    public ResponsePacket(byte requestId, byte statusCode, byte[] payload)
    {
      RequestId = requestId;
      StatusCode = statusCode;
      Payload = payload;
    }
  }
}