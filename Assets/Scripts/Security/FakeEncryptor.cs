namespace Securiton.Security
{
  public sealed class FakeEncryptor : IEncryptor
  {
    public byte[] Encrypt(byte[] data) => data;
    public byte[] Decrypt(byte[] data) => data;
  }
}