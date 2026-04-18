using System.IO;
using Securiton.Domain;

namespace Securiton.Serialization
{
  /// <summary>
  /// Converts the response payload bytes into a SensorValue domain object.
  ///
  /// Expected payload format:
  /// [SensorValue: float]
  /// </summary>
  public sealed class SensorValueDeserializer : IResponseDeserializer<SensorValue>
  {
    public SensorValue Deserialize(byte[] payload)
    {
      using var stream = new MemoryStream(payload);
      using var reader = new BinaryReader(stream);

      float value = reader.ReadSingle();

      return new SensorValue(value);
    }
  }
}