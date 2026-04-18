using System.Collections.Generic;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  /// <summary>
  /// Verifies that a permission payload can be converted into
  /// a GroupPermission root object.
  /// </summary>
  public sealed class GroupPermissionDeserializerTests
  {
    [Test]
    public void Deserialize_ReturnsExpectedRootGroup()
    {
      // Arrange
      var root = new GroupPermission(
        "Root",
        new List<Permission>
        {
          new SimplePermission("CanRead", true),
          new AccessLevelPermission("DoorAccess", 1)
        });

      var permissionSerializer = new PermissionSerializer();
      byte[] payload = permissionSerializer.Serialize(root);

      var permissionDeserializer = new PermissionDeserializer();
      var groupPermissionDeserializer = new GroupPermissionDeserializer(permissionDeserializer);

      // Act
      GroupPermission result = groupPermissionDeserializer.Deserialize(payload);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Name, Is.EqualTo("Root"));
      Assert.That(result.Children.Count, Is.EqualTo(2));
      Assert.That(result.Children[0], Is.TypeOf<SimplePermission>());
      Assert.That(result.Children[1], Is.TypeOf<AccessLevelPermission>());
    }
  }
}