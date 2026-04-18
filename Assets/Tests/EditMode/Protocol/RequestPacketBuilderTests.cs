using System.IO;
using NUnit.Framework;
using Securiton.Protocol;

namespace Securiton.Tests.EditMode.Protocol
{
  public sealed class RequestPacketBuilderTests
  {
    [Test]
    public void Build_WritesRequestIdPayloadLengthAndPayload()
    {
      var builder = new RequestPacketBuilder();
      byte[] payload = { 1, 2, 3 };

      byte[] packet = builder.Build(0x02, payload);

      Assert.That(packet, Is.Not.Null);

      using var stream = new MemoryStream(packet);
      using var reader = new BinaryReader(stream);

      byte requestId = reader.ReadByte();
      int payloadLength = reader.ReadInt32();
      byte[] actualPayload = reader.ReadBytes(payloadLength);

      Assert.That(requestId, Is.EqualTo(0x02));
      Assert.That(payloadLength, Is.EqualTo(3));
      Assert.That(actualPayload, Is.EqualTo(payload));
    }

    [Test]
    public void Build_WithNullPayload_WritesZeroLengthPayload()
    {
      var builder = new RequestPacketBuilder();

      byte[] packet = builder.Build(0x02, null);

      using var stream = new MemoryStream(packet);
      using var reader = new BinaryReader(stream);

      byte requestId = reader.ReadByte();
      int payloadLength = reader.ReadInt32();
      byte[] actualPayload = reader.ReadBytes(payloadLength);

      Assert.That(requestId, Is.EqualTo(0x02));
      Assert.That(payloadLength, Is.EqualTo(0));
      Assert.That(actualPayload.Length, Is.EqualTo(0));
    }
  }
}