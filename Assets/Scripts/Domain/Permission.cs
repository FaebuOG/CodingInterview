namespace Securiton.Domain
{
  public abstract class Permission
  {
    public string Name { get; }

    protected Permission(string name)
    {
      Name = name;
    }
  }
}