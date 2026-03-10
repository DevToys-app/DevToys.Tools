using System.Security.Cryptography;
using System.Text;
using DevToys.Tools.Models;

namespace DevToys.Tools.Helpers;
internal static class UuidHelper
{
    private enum GuidVersion
    {
        TimeBased = 0x01,
        Reserved = 0x02,
        NameBased = 0x03,
        Random = 0x04,
        NameBasedV5 = 0x05,
        UuidV6 = 0x06,
        UnixEpoch = 0x07,
        UuidObjectId = 0x08
    }

    private static readonly Random random = new();
    private static readonly RandomNumberGenerator cRandom = RandomNumberGenerator.Create();

    private static readonly DateTimeOffset gregorianCalendarStart = new(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);
    private const int VariantByte = 8;
    private const int VariantByteMask = 0x3f;
    private const int VariantByteShift = 0x80;
    private const int VariantByteShiftRFC = 0x80;
    private const int VersionByte = 7;
    private const int VersionByteMask = 0x0f;
    private const int VersionByteShift = 4;
    private const int ByteArraySize = 16;
    private const byte TimestampByte = 0;
    private const byte NodeByte = 10;
    private const byte GuidClockSequenceByte = 8;
    private static readonly object incrementLock = new();
    private static int increment = new Random().Next();
    private static readonly byte[] ProcessUnique = GenerateProcessUnique();

    internal static string GenerateUuid(UuidVersion version, bool hyphens, bool uppercase)
    {
        string guidStringFormat;
        if (hyphens)
        {
            guidStringFormat = "D";
        }
        else
        {
            guidStringFormat = "N";
        }

        string? uuid;
        switch (version)
        {
            case UuidVersion.One:
                uuid = GenerateTimeBasedGuid().ToString(guidStringFormat);
                break;
            case UuidVersion.Seven:
                uuid = GenerateUUIDv7().ToString(guidStringFormat);
                break;
            case UuidVersion.ObjectId:
                uuid = GenerateUuidObjectId();
                break;
            default:
                uuid = Guid.NewGuid().ToString(guidStringFormat);
                break;
        }

        if (uppercase)
        {
            uuid = uuid.ToUpperInvariant();
        }

        return uuid;
    }

    private static Guid GenerateTimeBasedGuid()
    {
        DateTime dateTime = DateTime.UtcNow;
        long ticks = dateTime.Ticks - gregorianCalendarStart.Ticks;

        byte[] guid = new byte[ByteArraySize];
        byte[] timestamp = BitConverter.GetBytes(ticks);

        // copy node
        byte[]? nodes = GenerateNodeBytes();
        Array.Copy(nodes, 0, guid, NodeByte, Math.Min(6, nodes.Length));

        // copy clock sequence
        byte[]? clockSequence = GenerateClockSequenceBytes(dateTime);
        Array.Copy(clockSequence, 0, guid, GuidClockSequenceByte, Math.Min(2, clockSequence.Length));

        // copy timestamp
        Array.Copy(timestamp, 0, guid, TimestampByte, Math.Min(8, timestamp.Length));

        // set the variant
        guid[VariantByte] &= (byte)VariantByteMask;
        guid[VariantByte] |= (byte)VariantByteShift;

        // set the version
        guid[VersionByte] &= (byte)VersionByteMask;
        guid[VersionByte] |= (byte)((byte)GuidVersion.TimeBased << VersionByteShift);

        return new Guid(guid);
    }

    private static Guid GenerateUUIDv7()
    {
        // TTTTTTTT-TTTT-7aaa-rRRR-RRRRRRRRRRRR
        // T=unix_ts_ms 7=version a=rand_a r=variant R=rand_b 

        // RRRRRRRR-RRRR-RRRR-RRRR-RRRRRRRRRRRR
        byte[] guid = new byte[ByteArraySize];
        cRandom.GetBytes(guid);

        // timestamp
        // If more precise timestamping or monotonicity is needed,
        // the 12-bit space in rand_a and some space in rand_b can be used.
        DateTimeOffset dateTime = DateTimeOffset.UtcNow;
        long timestamp = dateTime.ToUnixTimeMilliseconds();

        // 48-bit big-endian unsigned number of the Unix Epoch timestamp in milliseconds.
        // TTTTTTTT-TTTT-****-****-************
        guid[0] = (byte)((timestamp >> 40) & 0xFF);
        guid[1] = (byte)((timestamp >> 32) & 0xFF);
        guid[2] = (byte)((timestamp >> 24) & 0xFF);
        guid[3] = (byte)((timestamp >> 16) & 0xFF);
        guid[4] = (byte)((timestamp >> 8) & 0xFF);
        guid[5] = (byte)(timestamp & 0xFF);

        // set the version
        // ********-****-7***-****-************
        guid[6] &= (byte)VersionByteMask;
        guid[6] |= (byte)((byte)GuidVersion.UnixEpoch << VersionByteShift);

        // set the variant
        // ********-****-****-r***-************
        guid[8] &= (byte)VariantByteMask;
        guid[8] |= (byte)VariantByteShiftRFC;

        return new Guid(BitConverter.ToString(guid).Replace("-", ""));
    }

    public static byte[] GenerateNodeBytes()
    {
        byte[]? node = new byte[6];

        random.NextBytes(node);
        return node;
    }

    public static byte[] GenerateClockSequenceBytes(DateTime dt)
    {
        DateTime utc = dt.ToUniversalTime();

        byte[]? bytes = BitConverter.GetBytes(utc.Ticks);

        if (bytes.Length == 0)
        {
            return [0x0, 0x0];
        }

        if (bytes.Length == 1)
        {
            return [0x0, bytes[0]];
        }

        return [bytes[0], bytes[1]];
    }

    /// <summary>
    /// Generate a Mongodb ObjectID
    /// </summary>
    /// <returns></returns>
    private static string GenerateUuidObjectId()
    {
        int time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int inc = GetIncrement();
        byte[] buffer = new byte[12];

        // 4-byte timestamp
        buffer[0] = (byte)(time >> 24);
        buffer[1] = (byte)(time >> 16);
        buffer[2] = (byte)(time >> 8);
        buffer[3] = (byte)time;

        // 5-byte process unique
        Buffer.BlockCopy(ProcessUnique, 0, buffer, 4, 5);

        // 3-byte counter
        buffer[9] = (byte)(inc >> 16);
        buffer[10] = (byte)(inc >> 8);
        buffer[11] = (byte)inc;
        return ByteArrayToHexString(buffer);
    }

    /// <summary>
    /// Retrieves and increments the internal counter in a thread-safe manner.
    /// </summary>
    /// <returns>Returns the current value of the counter</returns>
    private static int GetIncrement()
    {
        lock (incrementLock)
        {
            return increment++;
        }
    }

    /// <summary>
    /// Generates a unique identifier specific to the current process.
    /// </summary>
    /// <returns>Returns a byte array containing the unique identifier for the process.</returns>
    private static byte[] GenerateProcessUnique()
    {
        byte[] processUnique = new byte[5];
        cRandom.GetBytes(processUnique);
        return processUnique;
    }

    /// <summary>
    /// Converts a byte array to its hexadecimal string representation.
    /// </summary>
    /// <param name="bytes">The byte array to be converted to a hexadecimal string.</param>
    /// <returns>Returns a string containing the hexadecimal representation of the byte array.</returns>
    private static string ByteArrayToHexString(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
