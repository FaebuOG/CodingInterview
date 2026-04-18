using NUnit.Framework;
using Securiton.Domain;
using Securiton.Tests.EditMode.Api.DeviceService;

namespace Securiton.Tests.EditMode.Integration
{
  /// <summary>
  /// Integration test for the complete ReadAlarmConfiguration flow.
  ///
  /// Verifies that the response payload is parsed and converted
  /// back into an AlarmConfiguration domain object.
  /// </summary>
  public sealed class ReadAlarmConfigurationFlowTests
  {
    [Test]
    public void ReadAlarmConfiguration_ReturnsExpectedConfiguration()
    {
      // Arrange
      var service = DeviceServiceFlowTestFactory.Create();

      // Act
      AlarmConfiguration result = service.ReadAlarmConfiguration();

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Threshold, Is.EqualTo(10));
      Assert.That(result.ReactionTime, Is.EqualTo(5));
      Assert.That(result.IsEnabled, Is.True);
    }
  }
}