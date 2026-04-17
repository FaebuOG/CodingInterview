using NUnit.Framework;
using Securiton.Api;
using Securiton.Domain;
using Securiton.Infrastructure;
using Securiton.Protocol;
using Securiton.Security;
using Securiton.Serialization;
using Securiton.Transport;
using Serialization;

namespace Securiton.Tests.EditMode
{
  public sealed class DeviceServiceTests
  {
    [Test]
    public void WriteAlarmConfiguration_ReturnsSuccessfulAck()
    {
      var transport = new FakeTransport();
      var encryptor = new FakeEncryptor();
      var packetBuilder = new RequestPacketBuilder();
      var packetParser = new ResponsePacketParser();
      var client = new DeviceClient(transport, encryptor, packetBuilder, packetParser);

      var serializer = new AlarmConfigSerializer();
      var ackDeserializer = new AckResponseDeserializer();

      var service = new DeviceService(client, serializer, ackDeserializer);

      var configuration = new AlarmConfiguration(10, 5, true);

      AckResponse response = service.WriteAlarmConfiguration(configuration);

      Assert.That(response.Success, Is.True);
      Assert.That(response.ErrorCode, Is.EqualTo(0x00));
    }
  }
}