using Securiton.Domain;

namespace Securiton.Requests
{
  /// <summary>
  /// Request for reading the current user permissions from the device.
  ///
  /// This request has no payload.
  /// Only the RequestId is required.
  /// </summary>
  public sealed class ReadUserPermissionsRequest : IDeviceRequest<GroupPermission>
  {
    public const byte Id = 0x04;

    public byte RequestId => Id;
  }
}