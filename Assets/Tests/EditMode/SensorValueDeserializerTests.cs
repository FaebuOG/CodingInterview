using System.IO;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  /// <summary>
  /// Verifies that the sensor value response payload is correctly
  /// converted back into a SensorValue domain object.
  /// </summary>
  public sealed class SensorValueDeserializerTests
  {
    [Test]
    public void Deserialize_ReadsFloatValueCorrectly()
    {
      // Arrange
      byte[] payload;

      using (var stream = new MemoryStream())
      using (var writer = new BinaryWriter(stream))
      {
        writer.Write(42.5f);
        writer.Flush();
        payload = stream.ToArray();
      }

      var deserializer = new SensorValueDeserializer();

      // Act
      SensorValue result = deserializer.Deserialize(payload);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Value, Is.EqualTo(42.5f));
    }
  }
}