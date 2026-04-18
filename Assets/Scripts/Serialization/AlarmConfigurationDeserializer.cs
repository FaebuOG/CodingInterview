using System.IO;
using Securiton.Domain;

namespace Securiton.Serialization
{
  /// <summary>
  /// Converts the response payload bytes into an AlarmConfiguration object.
  ///
  /// Expected payload format:
  /// [Threshold: int][ReactionTime: int][IsEnabled: bool]
  /// </summary>
  public sealed class AlarmConfigurationDeserializer : IResponseDeserializer<AlarmConfiguration>
  {
    public AlarmConfiguration Deserialize(byte[] payload)
    {
      using var stream = new MemoryStream(payload);
      using var reader = new BinaryReader(stream);

      int threshold = reader.ReadInt32();
      int reactionTime = reader.ReadInt32();
      bool isEnabled = reader.ReadBoolean();

      return new AlarmConfiguration(threshold, reactionTime, isEnabled);
    }
  }
}