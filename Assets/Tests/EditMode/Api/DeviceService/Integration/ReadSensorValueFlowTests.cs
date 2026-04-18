using NUnit.Framework;
using Securiton.Domain;
using Securiton.Tests.EditMode.Api.DeviceService;

namespace Securiton.Tests.EditMode.Integration
{
  /// <summary>
  /// Integration test for the complete ReadSensorValue flow.
  ///
  /// Verifies that a request without payload travels through the full
  /// communication pipeline and returns a deserialized SensorValue.
  /// </summary>
  public sealed class ReadSensorValueFlowTests
  {
    [Test]
    public void ReadSensorValue_ReturnsExpectedSensorValue()
    {
      // Arrange
      var service = DeviceServiceFlowTestFactory.Create();

      // Act
      SensorValue result = service.ReadSensorValue();

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Value, Is.EqualTo(42.5f));
    }
  }
}