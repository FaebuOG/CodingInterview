using System.IO;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  /// <summary>
  /// Verifies that the alarm configuration response payload is correctly
  /// converted back into an AlarmConfiguration domain object.
  /// </summary>
  public sealed class AlarmConfigurationDeserializerTests
  {
    [Test]
    public void Deserialize_ReadsThresholdReactionTimeAndIsEnabledCorrectly()
    {
      // Arrange
      byte[] payload;

      using (var stream = new MemoryStream())
      using (var writer = new BinaryWriter(stream))
      {
        writer.Write(10);
        writer.Write(5);
        writer.Write(true);
        writer.Flush();
        payload = stream.ToArray();
      }

      var deserializer = new AlarmConfigurationDeserializer();

      // Act
      AlarmConfiguration result = deserializer.Deserialize(payload);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Threshold, Is.EqualTo(10));
      Assert.That(result.ReactionTime, Is.EqualTo(5));
      Assert.That(result.IsEnabled, Is.True);
    }
  }
}