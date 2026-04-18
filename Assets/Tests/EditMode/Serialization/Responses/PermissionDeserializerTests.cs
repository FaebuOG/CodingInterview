using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Securiton.Domain;
using Securiton.Serialization;

namespace Securiton.Tests.EditMode.Serialization.Responses
{
    /// <summary>
    /// Tests for the PermissionDeserializer.
    ///
    /// Verifies that raw byte payloads can be correctly converted into
    /// the appropriate Permission subtype and that recursive structures
    /// are reconstructed properly.
    /// </summary>
    public sealed class PermissionDeserializerTests
    {
        private PermissionSerializer _serializer;
        private PermissionDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            _serializer = new PermissionSerializer();
            _deserializer = new PermissionDeserializer();
        }

        // --------------------------------------------------
        // SimplePermission
        // --------------------------------------------------

        [Test]
        public void Deserialize_SimplePermission_ReturnsCorrectObject()
        {
            // Arrange
            var original = new SimplePermission("CanEdit", true);
            byte[] payload = _serializer.Serialize(original);

            // Act
            Permission result = _deserializer.Deserialize(payload);

            // Assert
            Assert.That(result, Is.TypeOf<SimplePermission>());

            var simple = (SimplePermission)result;
            Assert.That(simple.Name, Is.EqualTo("CanEdit"));
            Assert.That(simple.IsGranted, Is.True);
        }

        // --------------------------------------------------
        // AccessLevelPermission
        // --------------------------------------------------

        [Test]
        public void Deserialize_AccessLevelPermission_ReturnsCorrectObject()
        {
            // Arrange
            var original = new AccessLevelPermission("DoorAccess", 2);
            byte[] payload = _serializer.Serialize(original);

            // Act
            Permission result = _deserializer.Deserialize(payload);

            // Assert
            Assert.That(result, Is.TypeOf<AccessLevelPermission>());

            var access = (AccessLevelPermission)result;
            Assert.That(access.Name, Is.EqualTo("DoorAccess"));
            Assert.That(access.AccessLevel, Is.EqualTo((byte)2));
        }

        // --------------------------------------------------
        // GroupPermission (flat)
        // --------------------------------------------------

        [Test]
        public void Deserialize_GroupPermission_WithChildren_ReturnsCorrectStructure()
        {
            // Arrange
            var original = new GroupPermission(
                "Root",
                new List<Permission>
                {
                    new SimplePermission("CanRead", true),
                    new AccessLevelPermission("DoorAccess", 1)
                });

            byte[] payload = _serializer.Serialize(original);

            // Act
            Permission result = _deserializer.Deserialize(payload);

            // Assert
            Assert.That(result, Is.TypeOf<GroupPermission>());

            var group = (GroupPermission)result;

            Assert.That(group.Name, Is.EqualTo("Root"));
            Assert.That(group.Children.Count, Is.EqualTo(2));

            Assert.That(group.Children[0], Is.TypeOf<SimplePermission>());
            Assert.That(group.Children[1], Is.TypeOf<AccessLevelPermission>());
        }

        // --------------------------------------------------
        // Nested Group (recursive)
        // --------------------------------------------------

        [Test]
        public void Deserialize_NestedGroupPermission_ReconstructsFullHierarchy()
        {
            // Arrange
            var original = new GroupPermission(
                "Root",
                new List<Permission>
                {
                    new SimplePermission("CanRead", true),
                    new GroupPermission(
                        "Admin",
                        new List<Permission>
                        {
                            new AccessLevelPermission("Settings", 2)
                        })
                });

            byte[] payload = _serializer.Serialize(original);

            // Act
            Permission result = _deserializer.Deserialize(payload);

            // Assert
            Assert.That(result, Is.TypeOf<GroupPermission>());

            var root = (GroupPermission)result;

            Assert.That(root.Children.Count, Is.EqualTo(2));

            // First child
            var simple = (SimplePermission)root.Children[0];
            Assert.That(simple.Name, Is.EqualTo("CanRead"));
            Assert.That(simple.IsGranted, Is.True);

            // Nested group
            var admin = (GroupPermission)root.Children[1];
            Assert.That(admin.Name, Is.EqualTo("Admin"));
            Assert.That(admin.Children.Count, Is.EqualTo(1));

            var nested = (AccessLevelPermission)admin.Children[0];
            Assert.That(nested.Name, Is.EqualTo("Settings"));
            Assert.That(nested.AccessLevel, Is.EqualTo((byte)2));
        }

        // --------------------------------------------------
        // Error: Unknown Type
        // --------------------------------------------------

        [Test]
        public void Deserialize_WithUnknownType_ThrowsInvalidDataException()
        {
            // Arrange
            byte[] payload;

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((byte)0xFF); // invalid type
                writer.Write(0);          // empty name
                writer.Flush();
                payload = stream.ToArray();
            }

            // Act & Assert
            Assert.Throws<System.IO.InvalidDataException>(() =>
            {
                _deserializer.Deserialize(payload);
            });
        }

        // --------------------------------------------------
        // Error: Truncated Payload
        // --------------------------------------------------

        [Test]
        public void Deserialize_WithTruncatedPayload_ThrowsEndOfStreamException()
        {
            // Arrange
            var original = new SimplePermission("Test", true);
            byte[] payload = _serializer.Serialize(original);
        
            byte[] truncated = new byte[payload.Length - 2];
            Array.Copy(payload, truncated, truncated.Length);
        
            // Act & Assert
            Assert.Throws<System.IO.EndOfStreamException>(() =>
            {
                _deserializer.Deserialize(truncated);
            });
        }
    }
}