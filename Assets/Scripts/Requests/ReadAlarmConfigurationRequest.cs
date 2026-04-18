using Securiton.Domain;

namespace Securiton.Requests
{
  /// <summary>
  /// Request for reading the current alarm configuration from the device.
  ///
  /// This request has no payload.
  /// Only the RequestId is required.
  /// </summary>
  public sealed class ReadAlarmConfigurationRequest : IDeviceRequest<AlarmConfiguration>
  {
    public const byte Id = 0x01;

    public byte RequestId => Id;
  }
}