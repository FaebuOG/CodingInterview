using System.IO;
using Securiton.Domain;

namespace Securiton.Serialization
{
  public sealed class AckResponseDeserializer : IResponseDeserializer<AckResponse>
  {
    public AckResponse Deserialize(byte[] payload)
    {
      using var stream = new MemoryStream(payload);
      using var reader = new BinaryReader(stream);

      bool success = reader.ReadBoolean();
      byte errorCode = reader.ReadByte();

      return new AckResponse(success, errorCode);
    }
  }
}