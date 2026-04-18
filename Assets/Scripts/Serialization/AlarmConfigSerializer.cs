using System.IO;
using Securiton.Requests;

namespace Securiton.Serialization
{
  public sealed class AlarmConfigSerializer : IRequestSerializer<WriteAlarmConfigurationRequest>
  {
    public byte[] Serialize(WriteAlarmConfigurationRequest request)
    {
      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      writer.Write(request.Threshold);
      writer.Write(request.ReactionTime);
      writer.Write(request.IsEnabled);

      writer.Flush();
      return stream.ToArray();
    }
  }
}