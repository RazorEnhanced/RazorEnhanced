namespace Assistant
{
	internal class StealthSteps
	{
		private static int m_Count;
		private static bool m_Hidden = false;

		internal static int Count { get { return m_Count; } }
		internal static bool Counting { get { return m_Hidden; } }

		internal static void OnMove()
		{
			if (World.Player.Visible && m_Hidden)
			{
				m_Hidden = false;
				return;
			}

			if (m_Hidden && Engine.MainWindow.ChkStealth.Checked && World.Player != null)
			{
				if (m_Count == 0)
					RazorEnhanced.Misc.SendMessage(Language.Format(LocString.StealthStart), 33, false);

				m_Count++;
				RazorEnhanced.Misc.SendMessage(Language.Format(LocString.StealthSteps, m_Count), 33, false);
			}
		}

		internal static void Hide()
		{
			m_Hidden = true;
			m_Count = 0;
		}

		internal static void Unhide()
		{
			m_Hidden = false;
			m_Count = 0;
		}
	}
}
