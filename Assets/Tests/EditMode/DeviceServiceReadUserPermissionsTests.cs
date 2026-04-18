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
    /// Verifies that the read user permissions flow works end-to-end:
    /// request -> packet -> transport -> response parse -> deserialization.
    /// </summary>
    public sealed class DeviceServiceReadUserPermissionsTests
    {
        [Test]
        public void ReadUserPermissions_ReturnsExpectedPermissionTree()
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
            var readUserPermissionsSerializer = new ReadUserPermissionsRequestSerializer();

            var ackDeserializer = new AckResponseDeserializer();
            var sensorValueDeserializer = new SensorValueDeserializer();
            var alarmConfigurationDeserializer = new AlarmConfigurationDeserializer();

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
            GroupPermission response = service.ReadUserPermissions();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Name, Is.EqualTo("Root"));
            Assert.That(response.Children.Count, Is.EqualTo(3));

            Assert.That(response.Children[0], Is.TypeOf<SimplePermission>());
            Assert.That(response.Children[1], Is.TypeOf<AccessLevelPermission>());
            Assert.That(response.Children[2], Is.TypeOf<GroupPermission>());

            var simple = (SimplePermission)response.Children[0];
            Assert.That(simple.Name, Is.EqualTo("CanRead"));
            Assert.That(simple.IsGranted, Is.True);

            var access = (AccessLevelPermission)response.Children[1];
            Assert.That(access.Name, Is.EqualTo("DoorAccess"));
            Assert.That(access.AccessLevel, Is.EqualTo((byte)2));

            var reports = (GroupPermission)response.Children[2];
            Assert.That(reports.Name, Is.EqualTo("Reports"));
            Assert.That(reports.Children.Count, Is.EqualTo(1));
            Assert.That(reports.Children[0], Is.TypeOf<SimplePermission>());

            var nested = (SimplePermission)reports.Children[0];
            Assert.That(nested.Name, Is.EqualTo("CanExport"));
            Assert.That(nested.IsGranted, Is.False);
        }
    }
}