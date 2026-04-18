using Securiton.Infrastructure;
using Securiton.Protocol;
using Securiton.Requests;
using Securiton.Security;
using Securiton.Serialization;
using Securiton.Transport;

namespace Securiton.Tests.EditMode.Api.DeviceService
{
    /// <summary>
    /// Central factory for integration-style flow tests.
    ///
    /// Creates a fully wired DeviceService using the real serializers,
    /// packet builder/parser and fake transport/encryption.
    /// </summary>
    internal static class DeviceServiceFlowTestFactory
    {
        public static Securiton.Api.DeviceService Create()
        {
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

            return new Securiton.Api.DeviceService(
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
        }
    }
}