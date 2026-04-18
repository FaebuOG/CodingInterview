using System.IO;
using Securiton.Domain;

namespace Securiton.Serialization
{
  /// <summary>
  /// Converts the response payload bytes into a GroupPermission root object.
  ///
  /// Internally reuses PermissionDeserializer and validates that
  /// the top-level permission is actually a GroupPermission.
  /// </summary>
  public sealed class GroupPermissionDeserializer : IResponseDeserializer<GroupPermission>
  {
    private readonly PermissionDeserializer _permissionDeserializer;

    public GroupPermissionDeserializer(PermissionDeserializer permissionDeserializer)
    {
      _permissionDeserializer = permissionDeserializer;
    }

    public GroupPermission Deserialize(byte[] payload)
    {
      Permission permission = _permissionDeserializer.Deserialize(payload);

      if (permission is not GroupPermission groupPermission)
      {
        throw new InvalidDataException("Expected root permission to be a GroupPermission.");
      }

      return groupPermission;
    }
  }
}