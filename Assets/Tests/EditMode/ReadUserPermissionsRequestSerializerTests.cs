using NUnit.Framework;
using Securiton.Requests;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  /// <summary>
  /// Verifies that ReadUserPermissionsRequest produces an empty payload,
  /// because the request itself does not require request data.
  /// </summary>
  public sealed class ReadUserPermissionsRequestSerializerTests
  {
    [Test]
    public void Serialize_ReturnsEmptyPayload()
    {
      // Arrange
      var serializer = new ReadUserPermissionsRequestSerializer();
      var request = new ReadUserPermissionsRequest();

      // Act
      byte[] payload = serializer.Serialize(request);

      // Assert
      Assert.That(payload, Is.Not.Null);
      Assert.That(payload.Length, Is.EqualTo(0));
    }
  }
}