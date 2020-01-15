using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Assistant
{
	internal class ScreenCapManager
	{
		private static TimerCallback m_DoCaptureCall = new TimerCallback(CaptureNow);

		public static void Initialize()
		{
		}

		internal static void DeathCapture()
		{
				Timer.DelayedCallback(TimeSpan.FromSeconds(0.5), m_DoCaptureCall).Start();
		}

		internal static void CaptureNow()
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
				IntPtr hBmp = DLLImport.Razor.CaptureScreen(RazorEnhanced.Settings.General.ReadBool("CapFullScreen"), timestamp);
				using (Image img = Image.FromHbitmap(hBmp))
					img.Save(filename, GetFormat(type));
				DLLImport.Win.DeleteObject(hBmp);
			}
			catch
			{
			}

			Engine.MainWindow.ReloadScreenShotsList();
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


		private static readonly Object m_lock = new Object();
		internal static void DisplayTo(ListBox lb)
		{
			string path = RazorEnhanced.Settings.General.ReadString("CapPath");
			if (!Directory.Exists(path))
			{
				path = Assistant.Engine.RootPath; //Path.GetDirectoryName(Application.ExecutablePath);
				RazorEnhanced.Settings.General.WriteString("CapPath", path);
				Assistant.Engine.MainWindow.ScreenPath.Text = path;
			}

			if (lb.InvokeRequired)
			{
				lb.Invoke(new MethodInvoker(delegate
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
	}
}
