using Securiton.Domain;

namespace Securiton.Requests
{
  class WriteUserPermissionsRequest : IDeviceRequest<AckResponse>
  {
    public GroupPermission Root { get; }
    public byte RequestId { get; }
  }
}