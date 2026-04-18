using Securiton.Api;
using Securiton.Domain;
using Securiton.Infrastructure;
using Securiton.Protocol;
using Securiton.Requests;
using Securiton.Security;
using Securiton.Serialization;
using Securiton.Transport;

namespace Securiton.Tests.EditMode
{
  using NUnit.Framework;
  using System.Collections.Generic;

  public sealed class DeviceServiceWriteUserPermissionsTests
  {
    [Test]
    public void WriteUserPermissions_ReturnsSuccessfulAck()
    {
      var transport = new FakeTransport();
      var encryptor = new FakeEncryptor();
      var packetBuilder = new RequestPacketBuilder();
      var packetParser = new ResponsePacketParser();
      var client = new DeviceClient(transport, encryptor, packetBuilder, packetParser);

      var alarmSerializer = new AlarmConfigSerializer();
      var permissionSerializer = new PermissionSerializer();
      var userPermissionsRequestSerializer = new WriteUserPermissionsRequestSerializer(permissionSerializer);
      var ackDeserializer = new AckResponseDeserializer();

      var service = new DeviceService(
        client,
        alarmSerializer,
        userPermissionsRequestSerializer,
        ackDeserializer);

      var root = new GroupPermission(
        "Root",
        new List<Permission>
        {
          new SimplePermission("CanRead", true),
          new AccessLevelPermission("DoorAccess", 1)
        });

      AckResponse response = service.WriteUserPermissions(root);

      Assert.That(response.Success, Is.True);
      Assert.That(response.ErrorCode, Is.EqualTo(0x00));
    }
  }
}