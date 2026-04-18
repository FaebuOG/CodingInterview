using NUnit.Framework;
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
  /// <summary>
  /// Verifies that the read flow works end-to-end:
  /// request -> packet -> transport -> response parse -> deserialization.
  /// </summary>
  public sealed class DeviceServiceReadSensorValueTests
  {
    [Test]
    public void ReadSensorValue_ReturnsExpectedValue()
    {
      // Arrange
      var transport = new FakeTransport();
      var encryptor = new FakeEncryptor();
      var packetBuilder = new RequestPacketBuilder();
      var packetParser = new ResponsePacketParser();
      var client = new DeviceClient(transport, encryptor, packetBuilder, packetParser);

      var writeAlarmSerializer = new AlarmConfigSerializer();
      var permissionSerializer = new PermissionSerializer();
      var writeUserPermissionsSerializer = new WriteUserPermissionsRequestSerializer(permissionSerializer);
      var readSensorValueSerializer = new ReadSensorValueRequestSerializer();

      var ackDeserializer = new AckResponseDeserializer();
      var sensorValueDeserializer = new SensorValueDeserializer();

      var readAlarmConfigurationSerializer = new ReadAlarmConfigurationRequestSerializer();
      var alarmConfigurationDeserializer = new AlarmConfigurationDeserializer();

      var readUserPermissionsSerializer = new ReadUserPermissionsRequestSerializer();
      var permissionDeserializer = new PermissionDeserializer();
      var groupPermissionDeserializer = new GroupPermissionDeserializer(permissionDeserializer);
      
      var service = new DeviceService(
        client,
        writeAlarmSerializer,
        writeUserPermissionsSerializer,
        readSensorValueSerializer,
        readAlarmConfigurationSerializer,
        readUserPermissionsSerializer,
        ackDeserializer,
        sensorValueDeserializer,
        alarmConfigurationDeserializer,
        groupPermissionDeserializer);

      // Act
      SensorValue response = service.ReadSensorValue();

      // Assert
      Assert.That(response, Is.Not.Null);
      Assert.That(response.Value, Is.EqualTo(42.5f));
    }
  }
}