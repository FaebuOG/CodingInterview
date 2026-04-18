namespace Securiton.Domain
{
  /// <summary>
  /// Domain object representing the current sensor value returned by the device.
  /// </summary>
  public sealed class SensorValue
  {
    public float Value { get; }

    public SensorValue(float value)
    {
      Value = value;
    }
  }
}