using NUnit.Framework;
using Securiton.Domain;
using Securiton.Tests.EditMode.Api.DeviceService;

namespace Securiton.Tests.EditMode.Integration
{
  /// <summary>
  /// Integration test for the complete WriteAlarmConfiguration flow.
  ///
  /// Covers:
  /// request creation -> serialization -> packet build -> encryption ->
  /// transport -> response parsing -> response deserialization.
  /// </summary>
  public sealed class WriteAlarmConfigurationFlowTests
  {
    [Test]
    public void WriteAlarmConfiguration_WithValidConfiguration_ReturnsSuccessfulAck()
    {
      // Arrange
      var service = DeviceServiceFlowTestFactory.Create();
      var configuration = new AlarmConfiguration(10, 5, true);

      // Act
      AckResponse result = service.WriteAlarmConfiguration(configuration);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Success, Is.True);
      Assert.That(result.ErrorCode, Is.EqualTo(0x00));
    }
  }
}