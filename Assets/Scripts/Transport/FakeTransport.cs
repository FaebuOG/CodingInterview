using System.IO;

namespace Securiton.Transport
{
  public sealed class FakeTransport : ITransport
  {
    public byte[] SendAndReceive(byte[] data)
    {
      byte requestId = data[0];

      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      writer.Write(requestId);   // correlation
      writer.Write((byte)0x00);  // statusCode = OK

      byte[] payload = BuildAckPayload(success: true, errorCode: 0x00);
      writer.Write(payload.Length);
      writer.Write(payload);

      writer.Flush();
      return stream.ToArray();
    }

    private static byte[] BuildAckPayload(bool success, byte errorCode)
    {
      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      writer.Write(success);
      writer.Write(errorCode);

      writer.Flush();
      return stream.ToArray();
    }
  }
}