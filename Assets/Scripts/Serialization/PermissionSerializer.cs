using System.IO;
using System.Text;
using Securiton.Domain;

namespace Securiton.Serialization
{
  public sealed class PermissionSerializer
  {
    private const byte SimpleType = 0x01;
    private const byte AccessLevelType = 0x02;
    private const byte GroupType = 0x03;

    public byte[] Serialize(Permission permission)
    {
      using var stream = new MemoryStream();
      using var writer = new BinaryWriter(stream);

      WritePermission(writer, permission);

      writer.Flush();
      return stream.ToArray();
    }

    private void WritePermission(BinaryWriter writer, Permission permission)
    {
      switch (permission)
      {
        case SimplePermission simple:
          writer.Write(SimpleType);
          WriteString(writer, simple.Name);
          writer.Write(simple.IsGranted);
          break;

        case AccessLevelPermission access:
          writer.Write(AccessLevelType);
          WriteString(writer, access.Name);
          writer.Write(access.AccessLevel);
          break;

        case GroupPermission group:
          writer.Write(GroupType);
          WriteString(writer, group.Name);
          writer.Write(group.Children.Count);

          foreach (Permission child in group.Children)
          {
            WritePermission(writer, child);
          }
          break;

        default:
          throw new InvalidDataException($"Unsupported permission type: {permission.GetType().Name}");
      }
    }

    private static void WriteString(BinaryWriter writer, string value)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
      writer.Write(bytes.Length);
      writer.Write(bytes);
    }
  }
}