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
    /// Verifies that the read alarm configuration flow works end-to-end:
    /// request -> packet -> transport -> response parse -> deserialization.
    /// </summary>
    public sealed class DeviceServiceReadAlarmConfigurationTests
    {
        [Test]
        public void ReadAlarmConfiguration_ReturnsExpectedConfiguration()
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
            var readAlarmConfigurationSerializer = new ReadAlarmConfigurationRequestSerializer();

            var ackDeserializer = new AckResponseDeserializer();
            var sensorValueDeserializer = new SensorValueDeserializer();
            var alarmConfigurationDeserializer = new AlarmConfigurationDeserializer();

            var service = new DeviceService(
                client,
                writeAlarmSerializer,
                writeUserPermissionsSerializer,
                readSensorValueSerializer,
                readAlarmConfigurationSerializer,
                ackDeserializer,
                sensorValueDeserializer,
                alarmConfigurationDeserializer);

            // Act
            AlarmConfiguration response = service.ReadAlarmConfiguration();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Threshold, Is.EqualTo(10));
            Assert.That(response.ReactionTime, Is.EqualTo(5));
            Assert.That(response.IsEnabled, Is.True);
        }
    }
}