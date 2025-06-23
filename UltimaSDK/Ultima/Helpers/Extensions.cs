using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Ultima.Helpers
{
    public static class Extensions
    {
        public static byte[] ToArray(this Bitmap bmp, PixelFormat? format = null)
        {
            if (bmp == null)
            {
                throw new ArgumentNullException(nameof(bmp));
            }

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, format ?? bmp.PixelFormat);
            try
            {
                int size = data.Stride * bmp.Height;
                byte[] buffer = new byte[size];
                Marshal.Copy(data.Scan0, buffer, 0, size);
                return buffer;
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        static readonly SHA256 _sha256 = SHA256.Create();

        public static byte[] ToSha256(this byte[] buffer)
        {
            return _sha256.ComputeHash(buffer);
        }

        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                return false;

            dict.Add(key, value);
            return true;
        }
    }
}
