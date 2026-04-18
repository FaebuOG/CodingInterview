using NUnit.Framework;
using Securiton.Requests;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  /// <summary>
  /// Verifies that ReadSensorValueRequest produces an empty payload,
  /// because the request does not require any request data.
  /// </summary>
  public sealed class ReadSensorValueRequestSerializerTests
  {
    [Test]
    public void Serialize_ReturnsEmptyPayload()
    {
      // Arrange
      var serializer = new ReadSensorValueRequestSerializer();
      var request = new ReadSensorValueRequest();

      // Act
      byte[] payload = serializer.Serialize(request);

      // Assert
      Assert.That(payload, Is.Not.Null);
      Assert.That(payload.Length, Is.EqualTo(0));
    }
  }
}