using Securiton.Domain;

namespace Securiton.Requests
{
  public sealed class WriteUserPermissionsRequest : IDeviceRequest<AckResponse>
  {
    public const byte Id = 0x05;

    public byte RequestId => Id;

    public GroupPermission RootPermissionGroup { get; }

    public WriteUserPermissionsRequest(GroupPermission rootPermissionGroup)
    {
      RootPermissionGroup = rootPermissionGroup;
    }
  }
}