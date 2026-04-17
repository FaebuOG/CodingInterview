namespace Securiton
{
  public interface IRequestPacketBuilder
  {
    byte[] Build(byte requestId, byte[] payload);
  }
}