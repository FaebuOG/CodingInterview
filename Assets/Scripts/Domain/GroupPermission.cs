using System.Collections.Generic;

namespace Securiton.Domain
{
  class GroupPermission : Permission
  {
    public List<Permission> Children { get; }
  }
}