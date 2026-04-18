using System.IO;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  public sealed class AckResponseDeserializerTests
  {
    [Test]
    public void Deserialize_ReadsSuccessAndErrorCodeCorrectly()
    {
      byte[] payload;

      using (var stream = new MemoryStream())
      using (var writer = new BinaryWriter(stream))
      {
        writer.Write(true);
        writer.Write((byte)0x00);
        writer.Flush();
        payload = stream.ToArray();
      }

      var deserializer = new AckResponseDeserializer();

      AckResponse response = deserializer.Deserialize(payload);

      Assert.That(response.Success, Is.True);
      Assert.That(response.ErrorCode, Is.EqualTo(0x00));
    }

    [Test]
    public void Deserialize_ReadsFailureAndErrorCodeCorrectly()
    {
      byte[] payload;

      using (var stream = new MemoryStream())
      using (var writer = new BinaryWriter(stream))
      {
        writer.Write(false);
        writer.Write((byte)0x42);
        writer.Flush();
        payload = stream.ToArray();
      }

      var deserializer = new AckResponseDeserializer();

      AckResponse response = deserializer.Deserialize(payload);

      Assert.That(response.Success, Is.False);
      Assert.That(response.ErrorCode, Is.EqualTo(0x42));
    }
  }
}