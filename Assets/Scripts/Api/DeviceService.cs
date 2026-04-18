using Securiton.Domain;
using Securiton.Infrastructure;
using Securiton.Requests;
using Securiton.Serialization;
using Serialization;

namespace Securiton.Api
{
    /// <summary>
    /// Application-facing API for device communication.
    ///
    /// This service exposes typed use cases and delegates the technical
    /// communication pipeline to DeviceClient.
    /// </summary>
    public sealed class DeviceService
    {
        private readonly DeviceClient _client;

        private readonly IRequestSerializer<WriteAlarmConfigurationRequest> _alarmSerializer;
        private readonly IRequestSerializer<WriteUserPermissionsRequest> _userPermissionsSerializer;
        private readonly IRequestSerializer<ReadSensorValueRequest> _readSensorValueSerializer;

        private readonly IResponseDeserializer<AckResponse> _ackDeserializer;
        private readonly IResponseDeserializer<SensorValue> _sensorValueDeserializer;

        public DeviceService(
            DeviceClient client,
            IRequestSerializer<WriteAlarmConfigurationRequest> alarmSerializer,
            IRequestSerializer<WriteUserPermissionsRequest> userPermissionsSerializer,
            IRequestSerializer<ReadSensorValueRequest> readSensorValueSerializer,
            IResponseDeserializer<AckResponse> ackDeserializer,
            IResponseDeserializer<SensorValue> sensorValueDeserializer)
        {
            _client = client;
            _alarmSerializer = alarmSerializer;
            _userPermissionsSerializer = userPermissionsSerializer;
            _readSensorValueSerializer = readSensorValueSerializer;
            _ackDeserializer = ackDeserializer;
            _sensorValueDeserializer = sensorValueDeserializer;
        }

        public AckResponse WriteAlarmConfiguration(AlarmConfiguration configuration)
        {
            var request = new WriteAlarmConfigurationRequest(configuration);

            return _client.Send<WriteAlarmConfigurationRequest, AckResponse>(
                request,
                _alarmSerializer,
                _ackDeserializer);
        }

        public AckResponse WriteUserPermissions(GroupPermission rootPermissionGroup)
        {
            var request = new WriteUserPermissionsRequest(rootPermissionGroup);

            return _client.Send<WriteUserPermissionsRequest, AckResponse>(
                request,
                _userPermissionsSerializer,
                _ackDeserializer);
        }

        public SensorValue ReadSensorValue()
        {
            var request = new ReadSensorValueRequest();

            return _client.Send<ReadSensorValueRequest, SensorValue>(
                request,
                _readSensorValueSerializer,
                _sensorValueDeserializer);
        }
    }
}