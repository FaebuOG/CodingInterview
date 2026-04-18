namespace Securiton.Domain
{
  using System;

  public sealed class AccessLevelPermission : Permission
  {
    public byte AccessLevel { get; }

    public AccessLevelPermission(string name, byte accessLevel) : base(name)
    {
      if (accessLevel > 2)
        throw new ArgumentOutOfRangeException(nameof(accessLevel), "Access level must be 0, 1 or 2.");

      AccessLevel = accessLevel;
    }
  }
}