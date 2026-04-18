using Securiton.Requests;

namespace Securiton.Serialization
{
  /// <summary>
  /// Serializer for ReadUserPermissionsRequest.
  ///
  /// The request does not contain payload data,
  /// so this serializer returns an empty byte array.
  /// </summary>
  public sealed class ReadUserPermissionsRequestSerializer : IRequestSerializer<ReadUserPermissionsRequest>
  {
    public byte[] Serialize(ReadUserPermissionsRequest request)
    {
      return System.Array.Empty<byte>();
    }
  }
}