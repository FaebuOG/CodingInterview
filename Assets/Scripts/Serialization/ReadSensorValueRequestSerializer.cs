using Securiton.Requests;

namespace Securiton.Serialization
{
  /// <summary>
  /// Serializer for ReadSensorValueRequest.
  ///
  /// The request does not contain any payload data,
  /// so this serializer returns an empty byte array.
  /// </summary>
  public sealed class ReadSensorValueRequestSerializer : IRequestSerializer<ReadSensorValueRequest>
  {
    public byte[] Serialize(ReadSensorValueRequest request)
    {
      return System.Array.Empty<byte>();
    }
  }
}