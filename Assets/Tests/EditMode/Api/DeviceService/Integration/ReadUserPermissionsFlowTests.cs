using NUnit.Framework;
using Securiton.Domain;
using Securiton.Tests.EditMode.Api.DeviceService;

namespace Securiton.Tests.EditMode.Integration
{
  /// <summary>
  /// Integration test for the complete ReadUserPermissions flow.
  ///
  /// Verifies recursive deserialization of a hierarchical permission tree
  /// returned by the fake device transport.
  /// </summary>
  public sealed class ReadUserPermissionsFlowTests
  {
    [Test]
    public void ReadUserPermissions_ReturnsExpectedPermissionHierarchy()
    {
      // Arrange
      var service = DeviceServiceFlowTestFactory.Create();

      // Act
      GroupPermission result = service.ReadUserPermissions();

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Name, Is.EqualTo("Root"));
      Assert.That(result.Children.Count, Is.EqualTo(3));

      Assert.That(result.Children[0], Is.TypeOf<SimplePermission>());
      Assert.That(result.Children[1], Is.TypeOf<AccessLevelPermission>());
      Assert.That(result.Children[2], Is.TypeOf<GroupPermission>());

      var simple = (SimplePermission)result.Children[0];
      Assert.That(simple.Name, Is.EqualTo("CanRead"));
      Assert.That(simple.IsGranted, Is.True);

      var access = (AccessLevelPermission)result.Children[1];
      Assert.That(access.Name, Is.EqualTo("DoorAccess"));
      Assert.That(access.AccessLevel, Is.EqualTo((byte)2));

      var group = (GroupPermission)result.Children[2];
      Assert.That(group.Name, Is.EqualTo("Reports"));
      Assert.That(group.Children.Count, Is.EqualTo(1));
      Assert.That(group.Children[0], Is.TypeOf<SimplePermission>());

      var nested = (SimplePermission)group.Children[0];
      Assert.That(nested.Name, Is.EqualTo("CanExport"));
      Assert.That(nested.IsGranted, Is.False);
    }
  }
}