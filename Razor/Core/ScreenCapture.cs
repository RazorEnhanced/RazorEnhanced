using Assistant.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Assistant
{
    internal class ScreenCapManager
    {
        private static readonly TimerCallback m_DoCaptureCall = new(CaptureNow);

        public static void Initialize()
        {
        }

        internal static void DeathCapture(double delay)
        {
            Timer.DelayedCallback(TimeSpan.FromSeconds(delay), m_DoCaptureCall).Start();
        }

        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        internal static void CaptureNow()
        {
            CaptureNowPath();
        }
        internal static string CaptureNowPath()
        {
            string filename;
            string timestamp;
            string name = "Unknown";
            string path = RazorEnhanced.Settings.General.ReadString("CapPath");
            string type = RazorEnhanced.Settings.General.ReadString("ImageFormat").ToLower();

            if (World.Player != null)
                name = World.Player.Name;
            if (name == null || name.Trim() == "" || name.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                name = "Unknown";

            if (RazorEnhanced.Settings.General.ReadBool("CapTimeStamp"))
                timestamp = String.Format("{0} ({1}) - {2}", name, World.ShardName, DateTime.Now.ToString(@"M/dd/yy - HH:mm:ss"));
            else
                timestamp = "";

            name = String.Format("{0}_{1}", name, DateTime.Now.ToString("M-d_HH.mm"));

            if (!Directory.Exists(path))
            {

                path = Assistant.Engine.RootPath; //  Path.GetDirectoryName(Application.ExecutablePath);
                RazorEnhanced.Settings.General.WriteString("CapPath", path);
                Assistant.Engine.MainWindow.ScreenPath.Text = path;
            }


            int count = 0;
            do
            {
                filename = Path.Combine(path, String.Format("{0}{1}.{2}", name, count != 0 ? count.ToString() : "", type));
                count--; // cause a - to be put in front of the number
            }
            while (File.Exists(filename));

            try
            {
                IntPtr handle = Client.Instance.GetWindowHandle();
                Image winImage = CaptureWindow(handle);
                winImage.Save(filename, GetFormat(type));
            }
            catch
            {
            }

            //Engine.MainWindow.ReloadScreenShotsList();
            Engine.MainWindow.SafeAction(s => s.ReloadScreenShotsList());
            return filename;
        }

        private static ImageFormat GetFormat(string fmt)
        {
            //string fmt = Config.GetString( "ImageFormat" ).ToLower();
            if (fmt == "jpeg" || fmt == "jpg")
                return ImageFormat.Jpeg;
            else if (fmt == "png")
                return ImageFormat.Png;
            else if (fmt == "bmp")
                return ImageFormat.Bmp;
            else if (fmt == "gif")
                return ImageFormat.Gif;
            else if (fmt == "tiff" || fmt == "tif")
                return ImageFormat.Tiff;
            else if (fmt == "wmf")
                return ImageFormat.Wmf;
            else if (fmt == "exif")
                return ImageFormat.Exif;
            else if (fmt == "emf")
                return ImageFormat.Emf;
            else
                return ImageFormat.Jpeg;
        }


        private static readonly Object m_lock = new();
        internal static void DisplayTo(ListBox lb)
        {
            string path = RazorEnhanced.Settings.General.ReadString("CapPath");
            if (!Directory.Exists(path))
            {
                path = Assistant.Engine.RootPath; //Path.GetDirectoryName(Application.ExecutablePath);
                RazorEnhanced.Settings.General.WriteString("CapPath", path);
                Assistant.Engine.MainWindow.ScreenPath.Text = path;
            }

            // Credzba look here
            if (lb.InvokeRequired)
            {
                lb.BeginInvoke(new MethodInvoker(delegate
                {
                    lock (m_lock)
                    {
                        lb.BeginUpdate();
                        lb.Items.Clear();
                        AddFiles(lb, path, "jpeg");
                        AddFiles(lb, path, "jpg");
                        AddFiles(lb, path, "png");
                        AddFiles(lb, path, "bmp");
                        AddFiles(lb, path, "gif");
                        AddFiles(lb, path, "tiff");
                        AddFiles(lb, path, "tif");
                        AddFiles(lb, path, "wmf");
                        AddFiles(lb, path, "exif");
                        AddFiles(lb, path, "emf");
                        lb.EndUpdate();
                    }
                }));
            }
            else
            {
                lock (m_lock)
                {
                    lb.BeginUpdate();
                    lb.Items.Clear();
                    AddFiles(lb, path, "jpeg");
                    AddFiles(lb, path, "jpg");
                    AddFiles(lb, path, "png");
                    AddFiles(lb, path, "bmp");
                    AddFiles(lb, path, "gif");
                    AddFiles(lb, path, "tiff");
                    AddFiles(lb, path, "tif");
                    AddFiles(lb, path, "wmf");
                    AddFiles(lb, path, "exif");
                    AddFiles(lb, path, "emf");
                    lb.EndUpdate();
                }
            }
        }

        internal static void AddFiles(ListBox list, string path, string ext)
        {
            string[] files = Directory.GetFiles(path, String.Format("*.{0}", ext));
            for (int i = 0; i < files.Length && list.Items.Count < 500; i++)
                list.Items.Add(Path.GetFileName(files[i]));
        }

        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
    }
}

