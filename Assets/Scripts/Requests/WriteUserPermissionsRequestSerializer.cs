using Securiton.Serialization;

namespace Securiton.Requests
{
  public sealed class WriteUserPermissionsRequestSerializer : IRequestSerializer<WriteUserPermissionsRequest>
  {
    private readonly PermissionSerializer _permissionSerializer;

    public WriteUserPermissionsRequestSerializer(PermissionSerializer permissionSerializer)
    {
      _permissionSerializer = permissionSerializer;
    }

    public byte[] Serialize(WriteUserPermissionsRequest request)
    {
      return _permissionSerializer.Serialize(request.RootPermissionGroup);
    }
  }
}