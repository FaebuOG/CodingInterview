using System.Collections.Generic;

namespace Securiton.Domain
{
  using System.Collections.Generic;

  public sealed class GroupPermission : Permission
  {
    public List<Permission> Children { get; }

    public GroupPermission(string name, List<Permission> children) : base(name)
    {
      Children = children ?? new List<Permission>();
    }
  }
}