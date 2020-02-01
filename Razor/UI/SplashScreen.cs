using System.Threading;
using System.Windows.Forms;

namespace Assistant
{
	internal class SplashScreen : System.Windows.Forms.Form
	{
		private static SplashScreen m_Screen;
		internal static SplashScreen Instance { get { return m_Screen; } }

		internal static void Start()
		{
			if (m_Screen != null)
				return;

			Thread t = new Thread(new ThreadStart(ThreadMain)) {Name = "Razor Splash Screen"};
			t.Start();
			while (m_Screen == null)
				Thread.Sleep(1);

			m_Screen.CreateControl();
		}

		private delegate void CloseDelegate();

		internal static void End()
		{
			if (m_Screen == null)
				return;

			if (m_Screen.InvokeRequired)
				m_Screen.Invoke(new CloseDelegate(m_Screen.Close));
			else
				m_Screen.Close();
		}

		private delegate void SetMsgDelegate(SplashScreen screen, string arg);

		private static void ThreadMain()
		{
			try
			{
				using (m_Screen = new SplashScreen())
				{
					Application.Run(m_Screen);
				}
				m_Screen = null;
			}
			catch
			{
			}
		}

		private System.ComponentModel.Container components = null;

		internal SplashScreen()
		{
			try
			{
				InitializeComponent();
			}
			catch
			{
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
			this.SuspendLayout();
			//
			// SplashScreen
			//
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.BackColor = System.Drawing.Color.Maroon;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(600, 333);
			this.ControlBox = false;
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashScreen";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Razor Splash Screen";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.Maroon;
			this.Load += new System.EventHandler(this.SplashScreen_Load);
			this.ResumeLayout(false);

		}

		#endregion Windows Form Designer generated code

		private void SplashScreen_Load(object sender, System.EventArgs e)
		{
			this.Activate();
			this.BringToFront();
			this.Focus();
		}
	}
}
