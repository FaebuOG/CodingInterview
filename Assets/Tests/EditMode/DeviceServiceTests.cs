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
  public sealed class DeviceServiceTests
  {
    [Test]
    public void WriteAlarmConfiguration_ReturnsSuccessfulAck()
    {
      // Arrange
      var transport = new FakeTransport();
      var encryptor = new FakeEncryptor();
      var packetBuilder = new RequestPacketBuilder();
      var packetParser = new ResponsePacketParser();
      var client = new DeviceClient(transport, encryptor, packetBuilder, packetParser);

      var alarmSerializer = new AlarmConfigSerializer();
      var permissionSerializer = new PermissionSerializer();
      var userPermissionsSerializer = new WriteUserPermissionsRequestSerializer(permissionSerializer);
      var readSensorValueSerializer = new ReadSensorValueRequestSerializer();

      var ackDeserializer = new AckResponseDeserializer();
      var sensorValueDeserializer = new SensorValueDeserializer();

      var readAlarmConfigurationSerializer = new ReadAlarmConfigurationRequestSerializer();
      var alarmConfigurationDeserializer = new AlarmConfigurationDeserializer();

      var service = new DeviceService(
        client,
        alarmSerializer,
        userPermissionsSerializer,
        readSensorValueSerializer,
        readAlarmConfigurationSerializer,
        ackDeserializer,
        sensorValueDeserializer,
        alarmConfigurationDeserializer);

      var configuration = new AlarmConfiguration(10, 5, true);

      // Act
      AckResponse response = service.WriteAlarmConfiguration(configuration);

      // Assert
      Assert.That(response.Success, Is.True);
      Assert.That(response.ErrorCode, Is.EqualTo(0x00));
    }
  }
}