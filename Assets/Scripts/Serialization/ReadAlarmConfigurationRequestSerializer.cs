using Securiton.Requests;

namespace Securiton.Serialization
{
  /// <summary>
  /// Serializer for ReadAlarmConfigurationRequest.
  ///
  /// The request does not contain payload data,
  /// so this serializer returns an empty byte array.
  /// </summary>
  public sealed class ReadAlarmConfigurationRequestSerializer : IRequestSerializer<ReadAlarmConfigurationRequest>
  {
    public byte[] Serialize(ReadAlarmConfigurationRequest request)
    {
      return System.Array.Empty<byte>();
    }
  }
}