using Securiton.Domain;

namespace Securiton.Serialization
{
  using System.Collections.Generic;
  using System.IO;
  using System.Text;

  public sealed class PermissionDeserializer
  {
    private const byte SimpleType = 0x01;
    private const byte AccessLevelType = 0x02;
    private const byte GroupType = 0x03;

    public Permission Deserialize(byte[] data)
    {
      using var stream = new MemoryStream(data);
      using var reader = new BinaryReader(stream);

      return ReadPermission(reader);
    }

    private Permission ReadPermission(BinaryReader reader)
    {
      byte type = reader.ReadByte();
      string name = ReadString(reader);

      return type switch
      {
        SimpleType => new SimplePermission(name, reader.ReadBoolean()),
        AccessLevelType => new AccessLevelPermission(name, reader.ReadByte()),
        GroupType => ReadGroup(reader, name),
        _ => throw new InvalidDataException($"Unknown permission type: {type}")
      };
    }

    private Permission ReadGroup(BinaryReader reader, string name)
    {
      int childCount = reader.ReadInt32();
      var children = new List<Permission>(childCount);

      for (int i = 0; i < childCount; i++)
      {
        children.Add(ReadPermission(reader));
      }

      return new GroupPermission(name, children);
    }

    private static string ReadString(BinaryReader reader)
    {
      int length = reader.ReadInt32();
      byte[] bytes = reader.ReadBytes(length);
      return Encoding.UTF8.GetString(bytes);
    }
  }
}