using System.Collections.Generic;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
  public sealed class PermissionRoundtripTests
  {
    [Test]
    public void NestedGroupPermission_Roundtrip_PreservesStructure()
    {
      var original = new GroupPermission(
        "Root",
        new List<Permission>
        {
          new SimplePermission("CanRead", true),
          new AccessLevelPermission("DoorAccess", 2),
          new GroupPermission(
            "Admin",
            new List<Permission>
            {
              new SimplePermission("CanEdit", true)
            })
        });

      var serializer = new PermissionSerializer();
      var deserializer = new PermissionDeserializer();

      byte[] bytes = serializer.Serialize(original);
      Permission result = deserializer.Deserialize(bytes);

      Assert.That(result, Is.TypeOf<GroupPermission>());

      var root = (GroupPermission)result;
      Assert.That(root.Name, Is.EqualTo("Root"));
      Assert.That(root.Children.Count, Is.EqualTo(3));

      Assert.That(root.Children[0], Is.TypeOf<SimplePermission>());
      Assert.That(root.Children[1], Is.TypeOf<AccessLevelPermission>());
      Assert.That(root.Children[2], Is.TypeOf<GroupPermission>());
    }
  }
}