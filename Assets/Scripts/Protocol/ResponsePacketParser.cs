using System.IO;

namespace Securiton.Protocol
{
  public sealed class ResponsePacketParser : IResponsePacketParser
  {
    public ResponsePacket Parse(byte[] data)
    {
      using var stream = new MemoryStream(data);
      using var reader = new BinaryReader(stream);

      byte requestId = reader.ReadByte();
      byte statusCode = reader.ReadByte();
      int payloadLength = reader.ReadInt32();
      byte[] payload = reader.ReadBytes(payloadLength);

      return new ResponsePacket(requestId, statusCode, payload);
    }
  }
}