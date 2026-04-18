using Securiton.Domain;

namespace Securiton.Requests
{
  /// <summary>
  /// Request for reading the current sensor value from the device.
  ///
  /// This request has no payload.
  /// Only the RequestId is needed.
  /// </summary>
  public sealed class ReadSensorValueRequest : IDeviceRequest<SensorValue>
  {
    public const byte Id = 0x03;

    public byte RequestId => Id;
  }
}