using System.Collections.Generic;
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
    /// These tests verify that user permissions can travel through
    /// the full communication pipeline and return a valid acknowledgement.
    ///
    /// This is not only a serializer test.
    /// It is a pipeline integration test using fake transport/encryption.
    /// </summary>
    public sealed class DeviceServiceWriteUserPermissionsTests
    {
        [Test]
        public void WriteUserPermissions_WithMixedPermissionTypes_ReturnsSuccessfulAck()
        {
            // Arrange
            // Build the communication pipeline with fakes.
            var transport = new FakeTransport();
            var encryptor = new FakeEncryptor();
            var packetBuilder = new RequestPacketBuilder();
            var packetParser = new ResponsePacketParser();
            var client = new DeviceClient(transport, encryptor, packetBuilder, packetParser);

            // Build serializers/deserializers used by the service.
            var alarmSerializer = new AlarmConfigSerializer();

            // Serializes the hierarchical permission tree into the request payload.
            var permissionSerializer = new PermissionSerializer();
            var userPermissionsSerializer = new WriteUserPermissionsRequestSerializer(permissionSerializer);

            // ReadSensorValue dependencies are not used in this test directly,
            // but the DeviceService constructor now requires them.
            var readSensorValueSerializer = new ReadSensorValueRequestSerializer();
            var sensorValueDeserializer = new SensorValueDeserializer();
            
            var readAlarmConfigurationSerializer = new ReadAlarmConfigurationRequestSerializer();
            var alarmConfigurationDeserializer = new AlarmConfigurationDeserializer();

            // Converts acknowledgement payload bytes back into AckResponse.
            var ackDeserializer = new AckResponseDeserializer();


            var service = new DeviceService(
                client,
                alarmSerializer,
                userPermissionsSerializer,
                readSensorValueSerializer,
                readAlarmConfigurationSerializer,
                ackDeserializer,
                sensorValueDeserializer,
                alarmConfigurationDeserializer);

            // Create a realistic permission tree:
            // root group with a simple permission, an access-level permission,
            // and a nested group.
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
            AckResponse response = service.WriteUserPermissions(root);

            // Assert
            // The fake transport currently returns a successful acknowledgement
            // for the supported request id.
            Assert.That(response.Success, Is.True);
            Assert.That(response.ErrorCode, Is.EqualTo(0x00));
        }
    }
}