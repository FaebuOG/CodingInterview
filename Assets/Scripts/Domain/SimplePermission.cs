namespace Securiton.Domain
{
  public sealed class SimplePermission : Permission
  {
    public bool IsGranted { get; }

    public SimplePermission(string name, bool isGranted) : base(name)
    {
      IsGranted = isGranted;
    }
  }
}