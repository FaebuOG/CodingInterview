using System.IO;
using NUnit.Framework;
using Securiton.Protocol;

namespace Securiton.Tests.EditMode
{
  public sealed class ResponsePacketParserTests
  {
    [Test]
    public void Parse_ReadsRequestIdStatusCodeAndPayloadCorrectly()
    {
      byte[] rawResponse;

      using (var stream = new MemoryStream())
      using (var writer = new BinaryWriter(stream))
      {
        writer.Write((byte)0x02);                 // requestId
        writer.Write((byte)0x00);                 // statusCode
        writer.Write(2);                          // payload length
        writer.Write(new byte[] { 1, 0 });        // payload
        writer.Flush();
        rawResponse = stream.ToArray();
      }

      var parser = new ResponsePacketParser();

      ResponsePacket packet = parser.Parse(rawResponse);

      Assert.That(packet.RequestId, Is.EqualTo(0x02));
      Assert.That(packet.StatusCode, Is.EqualTo(0x00));
      Assert.That(packet.Payload, Is.EqualTo(new byte[] { 1, 0 }));
    }

    [Test]
    public void Parse_WithEmptyPayload_ReturnsEmptyPayloadArray()
    {
      byte[] rawResponse;

      using (var stream = new MemoryStream())
      using (var writer = new BinaryWriter(stream))
      {
        writer.Write((byte)0x02);
        writer.Write((byte)0x00);
        writer.Write(0); // payload length
        writer.Flush();
        rawResponse = stream.ToArray();
      }

      var parser = new ResponsePacketParser();

      ResponsePacket packet = parser.Parse(rawResponse);

      Assert.That(packet.RequestId, Is.EqualTo(0x02));
      Assert.That(packet.StatusCode, Is.EqualTo(0x00));
      Assert.That(packet.Payload, Is.Not.Null);
      Assert.That(packet.Payload.Length, Is.EqualTo(0));
    }
  }
}