using System.IO;
using Securiton.Api;
using Securiton.Requests;

namespace Securiton.Transport
{
    /// <summary>
    /// Fake transport used for tests.
    ///
    /// It simulates device behavior and returns deterministic responses
    /// for known request ids.
    /// </summary>
    public sealed class FakeTransport : ITransport
    {
        public byte[] SendAndReceive(byte[] data)
        {
            byte requestId = data[0];

            return requestId switch
            {
                ReadAlarmConfigurationRequest.Id => BuildAlarmConfigurationResponse(requestId, 10, 5, true),
                WriteAlarmConfigurationRequest.Id => BuildAckResponse(requestId, true, 0x00),
                ReadSensorValueRequest.Id => BuildSensorValueResponse(requestId, 42.5f),
                WriteUserPermissionsRequest.Id => BuildAckResponse(requestId, true, 0x00),
                _ => BuildAckResponse(requestId, false, 0xFF)
            };
        }

        private static byte[] BuildAckResponse(byte requestId, bool success, byte errorCode)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(requestId);
            writer.Write(success ? (byte)0x00 : (byte)0x01);

            byte[] payload = BuildAckPayload(success, errorCode);
            writer.Write(payload.Length);
            writer.Write(payload);

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] BuildAckPayload(bool success, byte errorCode)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(success);
            writer.Write(errorCode);

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] BuildSensorValueResponse(byte requestId, float sensorValue)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(requestId);
            writer.Write((byte)0x00);

            byte[] payload = BuildSensorValuePayload(sensorValue);
            writer.Write(payload.Length);
            writer.Write(payload);

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] BuildSensorValuePayload(float sensorValue)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(sensorValue);

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] BuildAlarmConfigurationResponse(byte requestId, int threshold, int reactionTime, bool isEnabled)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(requestId);
            writer.Write((byte)0x00);

            byte[] payload = BuildAlarmConfigurationPayload(threshold, reactionTime, isEnabled);
            writer.Write(payload.Length);
            writer.Write(payload);

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] BuildAlarmConfigurationPayload(int threshold, int reactionTime, bool isEnabled)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(threshold);
            writer.Write(reactionTime);
            writer.Write(isEnabled);

            writer.Flush();
            return stream.ToArray();
        }
    }
}