namespace Securiton.Serialization
{
  public interface IRequestSerializer<in TRequest>
  {
    byte[] Serialize(TRequest request);
  }
}