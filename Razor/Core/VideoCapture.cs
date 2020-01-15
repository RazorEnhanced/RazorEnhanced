using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Accord.Video;
using Accord.Video.FFMPEG;
namespace Assistant
{
	public class VideoCapture
	{
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
				path = Assistant.Engine.RootPath;
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

		internal static bool Record(int fps, int codec)
		{
			if (!Assistant.Engine.CDepPresent)
				return false;

			IntPtr uowindow = Client.Instance.GetWindowHandle();

			if (uowindow == IntPtr.Zero)
				return false;

			System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(uowindow);

			Rectangle screenBound = screen.Bounds;
			if (!DLLImport.Win.GetWindowRect(uowindow, out DLLImport.Win.RECT handleRect))
				return false;

			//Getting the intersection between the two rectangles
			Rectangle handleBound = new Rectangle(handleRect.Left + 6, handleRect.Top, handleRect.Right - handleRect.Left - 6 , handleRect.Bottom - handleRect.Top);

			m_ResX = (handleBound.Right - handleBound.Left)-5;
			m_ResY = (handleBound.Bottom - handleBound.Top)-5;

			if (m_ResX % 2 != 0)
				m_ResX--;

			if (m_ResY % 2 != 0)
				m_ResY--;

			string filename;
			string name = "Unknown";
			string path = RazorEnhanced.Settings.General.ReadString("VideoPath");

			if (!Directory.Exists(path))
			{
				path = Assistant.Engine.RootPath;
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
				m_videostream = new ScreenCaptureStream(handleBound, fps);
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
			if (!Assistant.Engine.CDepPresent)
				return;

			try
			{
				if (m_videostream != null)
				{
					m_videostream.SignalToStop();
					m_videostream.WaitForStop();
					m_videostream = null;
				}

				if (m_filewriter != null)
				{
					m_filewriter.Close();
					m_filewriter = null;
				}

			}
			catch { }
			m_recording = false;
		}
	}
}
