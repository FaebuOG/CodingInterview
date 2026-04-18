using System;
using System.Collections.Generic;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode
{
    /// <summary>
    /// These tests verify that permissions can be serialized to bytes
    /// and reconstructed back into the correct object structure.
    ///
    /// This is important because the device communication contract
    /// is byte-based, not object-based.
    /// </summary>
    public sealed class PermissionRoundtripTests
    {
        [Test]
        public void SimplePermission_Roundtrip_PreservesTypeNameAndGrantedFlag()
        {
            // Arrange
            // Build a simple yes/no permission.
            var original = new SimplePermission("CanEdit", true);

            var serializer = new PermissionSerializer();
            var deserializer = new PermissionDeserializer();

            // Act
            // Convert permission -> bytes -> permission again.
            byte[] bytes = serializer.Serialize(original);
            Permission result = deserializer.Deserialize(bytes);

            // Assert
            // The permission must still be a SimplePermission after roundtrip.
            Assert.That(result, Is.TypeOf<SimplePermission>());

            var simple = (SimplePermission)result;

            // The semantic data must stay unchanged.
            Assert.That(simple.Name, Is.EqualTo("CanEdit"));
            Assert.That(simple.IsGranted, Is.True);
        }

        [Test]
        public void AccessLevelPermission_Roundtrip_PreservesTypeNameAndAccessLevel()
        {
            // Arrange
            var original = new AccessLevelPermission("DoorAccess", 2);

            var serializer = new PermissionSerializer();
            var deserializer = new PermissionDeserializer();

            // Act
            byte[] bytes = serializer.Serialize(original);
            Permission result = deserializer.Deserialize(bytes);

            // Assert
            Assert.That(result, Is.TypeOf<AccessLevelPermission>());

            var access = (AccessLevelPermission)result;

            Assert.That(access.Name, Is.EqualTo("DoorAccess"));
            Assert.That(access.AccessLevel, Is.EqualTo((byte)2));
        }

        [Test]
        public void GroupPermission_Roundtrip_PreservesChildrenAndChildTypes()
        {
            // Arrange
            // A group may contain mixed permission types.
            var original = new GroupPermission(
                "Admin",
                new List<Permission>
                {
                    new SimplePermission("CanEdit", true),
                    new AccessLevelPermission("DoorAccess", 1)
                });

            var serializer = new PermissionSerializer();
            var deserializer = new PermissionDeserializer();

            // Act
            byte[] bytes = serializer.Serialize(original);
            Permission result = deserializer.Deserialize(bytes);

            // Assert
            Assert.That(result, Is.TypeOf<GroupPermission>());

            var group = (GroupPermission)result;

            // Group metadata must stay intact.
            Assert.That(group.Name, Is.EqualTo("Admin"));
            Assert.That(group.Children.Count, Is.EqualTo(2));

            // Child types must remain correct after deserialization.
            Assert.That(group.Children[0], Is.TypeOf<SimplePermission>());
            Assert.That(group.Children[1], Is.TypeOf<AccessLevelPermission>());

            // Child content must remain correct as well.
            var firstChild = (SimplePermission)group.Children[0];
            var secondChild = (AccessLevelPermission)group.Children[1];

            Assert.That(firstChild.Name, Is.EqualTo("CanEdit"));
            Assert.That(firstChild.IsGranted, Is.True);

            Assert.That(secondChild.Name, Is.EqualTo("DoorAccess"));
            Assert.That(secondChild.AccessLevel, Is.EqualTo((byte)1));
        }

        [Test]
        public void NestedGroupPermission_Roundtrip_PreservesFullHierarchy()
        {
            // Arrange
            // This test is the most important one because it proves recursion works.
            var original = new GroupPermission(
                "Root",
                new List<Permission>
                {
                    new SimplePermission("CanRead", true),
                    new GroupPermission(
                        "SubGroup",
                        new List<Permission>
                        {
                            new AccessLevelPermission("Settings", 2)
                        })
                });

            var serializer = new PermissionSerializer();
            var deserializer = new PermissionDeserializer();

            // Act
            byte[] bytes = serializer.Serialize(original);
            Permission result = deserializer.Deserialize(bytes);

            // Assert
            Assert.That(result, Is.TypeOf<GroupPermission>());

            var root = (GroupPermission)result;
            Assert.That(root.Name, Is.EqualTo("Root"));
            Assert.That(root.Children.Count, Is.EqualTo(2));

            // First child must still be a simple permission.
            Assert.That(root.Children[0], Is.TypeOf<SimplePermission>());
            var simple = (SimplePermission)root.Children[0];
            Assert.That(simple.Name, Is.EqualTo("CanRead"));
            Assert.That(simple.IsGranted, Is.True);

            // Second child must still be a nested group.
            Assert.That(root.Children[1], Is.TypeOf<GroupPermission>());
            var subGroup = (GroupPermission)root.Children[1];
            Assert.That(subGroup.Name, Is.EqualTo("SubGroup"));
            Assert.That(subGroup.Children.Count, Is.EqualTo(1));

            // Nested content must also remain intact.
            Assert.That(subGroup.Children[0], Is.TypeOf<AccessLevelPermission>());
            var nestedAccess = (AccessLevelPermission)subGroup.Children[0];
            Assert.That(nestedAccess.Name, Is.EqualTo("Settings"));
            Assert.That(nestedAccess.AccessLevel, Is.EqualTo((byte)2));
        }

        [Test]
        public void AccessLevelPermission_WithInvalidLevel_ThrowsArgumentOutOfRangeException()
        {
            // Arrange / Act / Assert
            // Domain validation test:
            // only 0, 1, 2 are allowed by the task.
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new AccessLevelPermission("DoorAccess", 3);
            });
        }
    }
}