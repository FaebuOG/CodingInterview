using Securiton.Domain;
using Securiton.Infrastructure;
using Securiton.Requests;
using Securiton.Serialization;
using Serialization;


namespace Securiton.Api
{
  public sealed class DeviceService
  {
    private readonly DeviceClient _client;
    private readonly IRequestSerializer<WriteAlarmConfigurationRequest> _alarmSerializer;
    private readonly IRequestSerializer<WriteUserPermissionsRequest> _userPermissionsSerializer;
    private readonly IResponseDeserializer<AckResponse> _ackDeserializer;

    public DeviceService(
      DeviceClient client,
      IRequestSerializer<WriteAlarmConfigurationRequest> alarmSerializer,
      IRequestSerializer<WriteUserPermissionsRequest> userPermissionsSerializer,
      IResponseDeserializer<AckResponse> ackDeserializer)
    {
      _client = client;
      _alarmSerializer = alarmSerializer;
      _userPermissionsSerializer = userPermissionsSerializer;
      _ackDeserializer = ackDeserializer;
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
  }
}