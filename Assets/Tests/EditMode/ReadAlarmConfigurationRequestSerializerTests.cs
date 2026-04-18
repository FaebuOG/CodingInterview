using NUnit.Framework;
using Securiton.Requests;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  /// <summary>
  /// Verifies that ReadAlarmConfigurationRequest produces an empty payload,
  /// because the request itself does not require request data.
  /// </summary>
  public sealed class ReadAlarmConfigurationRequestSerializerTests
  {
    [Test]
    public void Serialize_ReturnsEmptyPayload()
    {
      // Arrange
      var serializer = new ReadAlarmConfigurationRequestSerializer();
      var request = new ReadAlarmConfigurationRequest();

      // Act
      byte[] payload = serializer.Serialize(request);

      // Assert
      Assert.That(payload, Is.Not.Null);
      Assert.That(payload.Length, Is.EqualTo(0));
    }
  }
}