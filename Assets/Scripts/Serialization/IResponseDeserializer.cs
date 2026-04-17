namespace Securiton.Serialization
{
  public interface IResponseDeserializer<out TResponse>
  {
    TResponse Deserialize(byte[] payload);
  }
}