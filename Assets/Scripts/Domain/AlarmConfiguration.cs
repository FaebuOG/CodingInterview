namespace Securiton.Domain
{
  public sealed class AlarmConfiguration
  {
    public int Threshold { get; }
    public int ReactionTime { get; }
    public bool IsEnabled { get; }

    public AlarmConfiguration(int threshold, int reactionTime, bool isEnabled)
    {
      Threshold = threshold;
      ReactionTime = reactionTime;
      IsEnabled = isEnabled;
    }
  }
}