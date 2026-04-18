using System.IO;
using Securiton.Requests;

namespace Securiton.Transport
{
  using System.IO;

  public sealed class FakeTransport : ITransport
  {
    public byte[] SendAndReceive(byte[] data)
    {
      byte requestId = data[0];

      return requestId switch
      {
        WriteAlarmConfigurationRequest.Id => BuildAckResponse(requestId, true, 0x00),
        WriteUserPermissionsRequest.Id => BuildAckResponse(requestId, true, 0x00),
        _ => BuildAckResponse(requestId, false, 0xFF)
      };
    }

    private static byte[] BuildAckResponse(byte requestId, bool success, byte errorCode)
    {
      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      writer.Write(requestId);
      writer.Write(success ? (byte)0x00 : (byte)0x01);

      byte[] payload = BuildAckPayload(success, errorCode);
      writer.Write(payload.Length);
      writer.Write(payload);

      writer.Flush();
      return stream.ToArray();
    }

    private static byte[] BuildAckPayload(bool success, byte errorCode)
    {
      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      writer.Write(success);
      writer.Write(errorCode);

      writer.Flush();
      return stream.ToArray();
    }
  }
}