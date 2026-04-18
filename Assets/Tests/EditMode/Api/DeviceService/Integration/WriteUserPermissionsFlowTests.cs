using System.Collections.Generic;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Tests.EditMode.Api.DeviceService;

namespace Securiton.Tests.EditMode.Integration
{
  /// <summary>
  /// Integration test for the complete WriteUserPermissions flow.
  ///
  /// Verifies recursive permission serialization and the full
  /// communication path up to the acknowledgement response.
  /// </summary>
  public sealed class WriteUserPermissionsFlowTests
  {
    [Test]
    public void WriteUserPermissions_WithMixedPermissionTree_ReturnsSuccessfulAck()
    {
      // Arrange
      var service = DeviceServiceFlowTestFactory.Create();

      var root = new GroupPermission(
        "Root",
        new List<Permission>
        {
          new SimplePermission("CanRead", true),
          new AccessLevelPermission("DoorAccess", 1),
          new GroupPermission(
            "Reports",
            new List<Permission>
            {
              new SimplePermission("CanExport", false)
            })
        });

      // Act
      AckResponse result = service.WriteUserPermissions(root);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Success, Is.True);
      Assert.That(result.ErrorCode, Is.EqualTo(0x00));
    }
  }
}