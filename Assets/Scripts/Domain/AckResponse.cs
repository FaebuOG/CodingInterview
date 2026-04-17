namespace Securiton.Domain
{
  public sealed class AckResponse
  {
    public bool Success { get; }
    public byte ErrorCode { get; }

    public AckResponse(bool success, byte errorCode)
    {
      Success = success;
      ErrorCode = errorCode;
    }
  }
}