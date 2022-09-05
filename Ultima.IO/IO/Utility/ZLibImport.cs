using System.Runtime.InteropServices;

namespace Ultima.Utility
{
	internal enum ZLibCompressionLevel : int
	{
		Z_NO_COMPRESSION = 0,
		Z_BEST_SPEED = 1,
		Z_BEST_COMPRESSION = 9,
		Z_DEFAULT_COMPRESSION = (-1)
	}
	internal enum ZLibError : int
	{
		Z_OK = 0,
		Z_STREAM_END = 1,
		Z_NEED_DICT = 2,
		Z_ERRNO = (-1),
		Z_STREAM_ERROR = (-2),
		Z_DATA_ERROR = (-3), // Data was corrupt
		Z_MEM_ERROR = (-4), //  Not Enough Memory
		Z_BUF_ERROR = (-5), // Not enough buffer space
		Z_VERSION_ERROR = (-6),
	}

	internal class ZLib
	{
		[DllImport("zlib")]
		internal static extern string zlibVersion();

		[DllImport("zlib")]
		internal static extern ZLibError compress(byte[] dest, ref int destLength, byte[] source, int sourceLength);

		[DllImport("zlib")]
		internal static extern ZLibError compress2(byte[] dest, ref int destLength, byte[] source, int sourceLength, ZLibCompressionLevel level);

		[DllImport("zlib")]
		internal static extern ZLibError uncompress(byte[] dest, ref int destLen, byte[] source, int sourceLen);
	}
}
