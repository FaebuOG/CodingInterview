using Securiton.Domain;
using Securiton.Infrastructure;
using Securiton.Requests;
using Securiton.Serialization;

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

        private readonly IRequestSerializer<WriteAlarmConfigurationRequest> _writeAlarmSerializer;
        private readonly IRequestSerializer<WriteUserPermissionsRequest> _writeUserPermissionsSerializer;
        private readonly IRequestSerializer<ReadSensorValueRequest> _readSensorValueSerializer;
        private readonly IRequestSerializer<ReadAlarmConfigurationRequest> _readAlarmConfigurationSerializer;

        private readonly IResponseDeserializer<AckResponse> _ackDeserializer;
        private readonly IResponseDeserializer<SensorValue> _sensorValueDeserializer;
        private readonly IResponseDeserializer<AlarmConfiguration> _alarmConfigurationDeserializer;

        public DeviceService(
            DeviceClient client,
            IRequestSerializer<WriteAlarmConfigurationRequest> writeAlarmSerializer,
            IRequestSerializer<WriteUserPermissionsRequest> writeUserPermissionsSerializer,
            IRequestSerializer<ReadSensorValueRequest> readSensorValueSerializer,
            IRequestSerializer<ReadAlarmConfigurationRequest> readAlarmConfigurationSerializer,
            IResponseDeserializer<AckResponse> ackDeserializer,
            IResponseDeserializer<SensorValue> sensorValueDeserializer,
            IResponseDeserializer<AlarmConfiguration> alarmConfigurationDeserializer)
        {
            _client = client;
            _writeAlarmSerializer = writeAlarmSerializer;
            _writeUserPermissionsSerializer = writeUserPermissionsSerializer;
            _readSensorValueSerializer = readSensorValueSerializer;
            _readAlarmConfigurationSerializer = readAlarmConfigurationSerializer;
            _ackDeserializer = ackDeserializer;
            _sensorValueDeserializer = sensorValueDeserializer;
            _alarmConfigurationDeserializer = alarmConfigurationDeserializer;
        }

        public AckResponse WriteAlarmConfiguration(AlarmConfiguration configuration)
        {
            var request = new WriteAlarmConfigurationRequest(configuration);

            return _client.Send<WriteAlarmConfigurationRequest, AckResponse>(
                request,
                _writeAlarmSerializer,
                _ackDeserializer);
        }

        public AckResponse WriteUserPermissions(GroupPermission rootPermissionGroup)
        {
            var request = new WriteUserPermissionsRequest(rootPermissionGroup);

            return _client.Send<WriteUserPermissionsRequest, AckResponse>(
                request,
                _writeUserPermissionsSerializer,
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

        public AlarmConfiguration ReadAlarmConfiguration()
        {
            var request = new ReadAlarmConfigurationRequest();

            return _client.Send<ReadAlarmConfigurationRequest, AlarmConfiguration>(
                request,
                _readAlarmConfigurationSerializer,
                _alarmConfigurationDeserializer);
        }
    }
}