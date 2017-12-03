using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Accord.Video;
using Accord.Video.FFMPEG;
namespace Assistant
{
	public class VideoCapture
	{
		[System.Runtime.InteropServices.DllImport( "Gdi32.dll" )]
		private static extern IntPtr DeleteObject( IntPtr hGdiObj );

		public static bool Recording { get { return m_recording; } }
		private static bool m_recording;
		private static int m_ResX, m_ResY;
		private static ScreenCaptureStream m_videostream;
		private static VideoFileWriter m_filewriter;

		internal static void DisplayTo(ListBox list)
		{
			string path = RazorEnhanced.Settings.General.ReadString("VideoPath");
			if (!Directory.Exists(path))
			{
				path = Path.GetDirectoryName(Application.ExecutablePath);
				RazorEnhanced.Settings.General.WriteString("VideoPath", path);
				Assistant.Engine.MainWindow.VideoPathTextBox.Text = path;
			}

			list.Items.Clear();
			AddFiles(list, path, "avi");
			AddFiles(list, path, "mp4");
		}

		internal static void AddFiles(ListBox list, string path, string ext)
		{
			if (list.Items.Count >= 500)
				return;

			string[] files = Directory.GetFiles(path, String.Format("*.{0}", ext));
			for (int i = 0; i < files.Length && list.Items.Count < 500; i++)
				list.Items.Add(Path.GetFileName(files[i]));
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;        // x position of upper-left corner
			public int Top;         // y position of upper-left corner
			public int Right;       // x position of lower-right corner
			public int Bottom;      // y position of lower-right corner
		}


		public static bool Record(int fps, int codec)
		{
			GetWindowRect(ClientCommunication.FindUOWindow(), out RECT lpRect);
			Rectangle screenArea = new Rectangle(lpRect.Left, lpRect.Top, (lpRect.Right - lpRect.Left), (lpRect.Bottom - lpRect.Top));
			foreach (System.Windows.Forms.Screen screen in
					  System.Windows.Forms.Screen.AllScreens)
			{
				screenArea = Rectangle.Union(screenArea, screen.Bounds);
			}

			m_ResX = (lpRect.Right - lpRect.Left)-5;
			m_ResY = (lpRect.Bottom - lpRect.Top)-5;

			if (m_ResX % 2 != 0)
				m_ResX--;

			if (m_ResY % 2 != 0)
				m_ResY--;

			string filename;
			string name = "Unknown";
			string path = RazorEnhanced.Settings.General.ReadString("VideoPath");

			if (!Directory.Exists(path))
			{
				path = Path.GetDirectoryName(Application.ExecutablePath);
				RazorEnhanced.Settings.General.WriteString("VideoPath", path);
				Assistant.Engine.MainWindow.VideoPathTextBox.Text = path;
			}

			if (World.Player != null)
				name = World.Player.Name;
			if (name == null || name.Trim() == "" || name.IndexOfAny(Path.GetInvalidPathChars()) != -1)
				name = "Unknown";

			name = String.Format("{0}_{1}", name, DateTime.Now.ToString("M-d_HH.mm"));

			int count = 0;
			do
			{
				filename = Path.Combine(path, String.Format("{0}{1}.avi", name, count != 0 ? count.ToString() : ""));
				count--; // cause a - to be put in front of the number 
			}
			while (File.Exists(filename));

			try
			{
				m_recording = true;
				m_filewriter = new VideoFileWriter();
				m_filewriter.Open(filename, m_ResX, m_ResY, fps, (VideoCodec)codec, 30000000);

				// create screen capture video source
				m_videostream = new ScreenCaptureStream(screenArea, fps);
				// set NewFrame event handler
				m_videostream.NewFrame += new NewFrameEventHandler(video_NewFrame);
				// start the video source
				m_videostream.Start();
				return true;
			}
			catch
			{
				MessageBox.Show("Video Codec not installed on your system.");
				return false;
			}
		}

		private static void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
		{
			m_filewriter.WriteVideoFrame(eventArgs.Frame);
		}

		public static void Stop()
		{
			try
			{
				if (m_videostream != null)
				{
					m_videostream.SignalToStop();
					m_videostream.WaitForStop();
					m_videostream = null;
				}


				if (m_filewriter != null)// && m_filewriter.IsOpen)
				{
					m_filewriter.Close();
					//m_filewriter.Dispose();
					m_filewriter = null;
				}
			}
			catch { }

			m_recording = false;
		}
	}
}
