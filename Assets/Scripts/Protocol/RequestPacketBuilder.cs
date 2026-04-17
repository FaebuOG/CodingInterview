using System.IO;


namespace Securiton.Protocol
{
  public sealed class RequestPacketBuilder : IRequestPacketBuilder
  {
    public byte[] Build(byte requestId, byte[] payload)
    {
      payload ??= System.Array.Empty<byte>();

      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      writer.Write(requestId);
      writer.Write(payload.Length);
      writer.Write(payload);

      writer.Flush();
      return stream.ToArray();
    }
  }
}