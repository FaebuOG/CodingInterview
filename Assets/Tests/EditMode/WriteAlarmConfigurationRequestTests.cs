using NUnit.Framework;
using Securiton.Domain;
using Securiton.Requests;

namespace Securiton.Tests.EditMode
{
  public sealed class WriteAlarmConfigurationRequestTests
  {
    [Test]
    public void Constructor_FromValues_SetsPropertiesCorrectly()
    {
      var request = new WriteAlarmConfigurationRequest(10, 5, true);

      Assert.That(request.RequestId, Is.EqualTo(0x02));
      Assert.That(request.Threshold, Is.EqualTo(10));
      Assert.That(request.ReactionTime, Is.EqualTo(5));
      Assert.That(request.IsEnabled, Is.True);
    }

    [Test]
    public void Constructor_FromAlarmConfiguration_CopiesValuesCorrectly()
    {
      var config = new AlarmConfiguration(20, 8, false);

      var request = new WriteAlarmConfigurationRequest(config);

      Assert.That(request.RequestId, Is.EqualTo(0x02));
      Assert.That(request.Threshold, Is.EqualTo(20));
      Assert.That(request.ReactionTime, Is.EqualTo(8));
      Assert.That(request.IsEnabled, Is.False);
    }
  }
}