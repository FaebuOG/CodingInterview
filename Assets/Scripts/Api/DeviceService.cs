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
    private readonly IResponseDeserializer<AckResponse> _ackDeserializer;

    public DeviceService(
      DeviceClient client,
      IRequestSerializer<WriteAlarmConfigurationRequest> alarmSerializer,
      IResponseDeserializer<AckResponse> ackDeserializer)
    {
      _client = client;
      _alarmSerializer = alarmSerializer;
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
  }
}