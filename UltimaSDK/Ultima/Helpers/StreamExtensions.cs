using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ultima.Helpers
{
    public static class StreamExtensions
    {
        public static byte[] ReadExactly(this Stream stream, int count)
        {
            var buffer = new byte[count];
            int offset = 0;

            while (offset < count)
            {
                int bytesRead = stream.Read(buffer, offset, count - offset);
                if (bytesRead == 0)
                    throw new EndOfStreamException($"Unable to read {count} bytes from stream. Only read {offset} bytes.");
                offset += bytesRead;
            }

            return buffer;
        }

        public static void ReadExactly(this Stream stream, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || count < 0 || (offset + count > buffer.Length))
                throw new ArgumentOutOfRangeException("Offset and count must be within the bounds of the buffer.");

            int totalRead = 0;
            while (totalRead < count)
            {
                int bytesRead = stream.Read(buffer, offset + totalRead, count - totalRead);
                if (bytesRead == 0)
                    throw new EndOfStreamException($"End of stream reached with {totalRead} of {count} bytes read.");
                totalRead += bytesRead;
            }
        }

        public static async Task<byte[]> ReadExactlyAsync(this Stream stream, int count, CancellationToken cancellationToken = default)
        {
            var buffer = new byte[count];
            int offset = 0;

            while (offset < count)
            {
                int bytesRead = await stream.ReadAsync(buffer, offset, count - offset, cancellationToken).ConfigureAwait(false);
                if (bytesRead == 0)
                    throw new EndOfStreamException($"Unable to read {count} bytes from stream. Only read {offset} bytes.");
                offset += bytesRead;
            }

            return buffer;
        }
    }
}
