namespace Securiton.Protocol
{
  public interface IResponsePacketParser
  {
    ResponsePacket Parse(byte[] data);
  }
}