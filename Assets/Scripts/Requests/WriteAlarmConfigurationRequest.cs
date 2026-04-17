using Securiton.Domain;

namespace Securiton.Requests
{
  public sealed class WriteAlarmConfigurationRequest : IDeviceRequest<AckResponse>
  {
    public const byte Id = 0x02;

    public byte RequestId => Id;

    public int Threshold { get; }
    public int ReactionTime { get; }
    public bool IsEnabled { get; }

    public WriteAlarmConfigurationRequest(int threshold, int reactionTime, bool isEnabled)
    {
      Threshold = threshold;
      ReactionTime = reactionTime;
      IsEnabled = isEnabled;
    }

    public WriteAlarmConfigurationRequest(AlarmConfiguration config)
      : this(config.Threshold, config.ReactionTime, config.IsEnabled)
    {
    }
  }
}