using System.IO;
using NUnit.Framework;
using Securiton.Requests;
using Securiton.Serialization;
using Serialization;

namespace Securiton.Tests.EditMode
{
  public sealed class AlarmConfigSerializerTests
  {
    [Test]
    public void Serialize_WritesThresholdReactionTimeAndIsEnabled_InCorrectOrder()
    {
      var serializer = new AlarmConfigSerializer();
      var request = new WriteAlarmConfigurationRequest(10, 5, true);

      byte[] bytes = serializer.Serialize(request);

      Assert.That(bytes, Is.Not.Null);
      Assert.That(bytes.Length, Is.EqualTo(9)); // int + int + bool

      using var stream = new MemoryStream(bytes);
      using var reader = new BinaryReader(stream);

      int threshold = reader.ReadInt32();
      int reactionTime = reader.ReadInt32();
      bool isEnabled = reader.ReadBoolean();

      Assert.That(threshold, Is.EqualTo(10));
      Assert.That(reactionTime, Is.EqualTo(5));
      Assert.That(isEnabled, Is.True);
    }

    [Test]
    public void Serialize_WritesFalseBooleanCorrectly()
    {
      var serializer = new AlarmConfigSerializer();
      var request = new WriteAlarmConfigurationRequest(1, 2, false);

      byte[] bytes = serializer.Serialize(request);

      using var stream = new MemoryStream(bytes);
      using var reader = new BinaryReader(stream);

      _ = reader.ReadInt32();
      _ = reader.ReadInt32();
      bool isEnabled = reader.ReadBoolean();

      Assert.That(isEnabled, Is.False);
    }
  }
}