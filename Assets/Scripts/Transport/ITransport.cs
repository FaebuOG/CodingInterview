namespace Securiton.Transport
{
  public interface ITransport
  {
    byte[] SendAndReceive(byte[] data);
  }
}