namespace Securiton.Requests
{
  public interface IDeviceRequest<TResponse>
  {
    byte RequestId { get; }
  }
}