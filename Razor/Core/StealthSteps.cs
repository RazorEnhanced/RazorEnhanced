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
			if (m_Hidden && RazorEnhanced.Settings.General.ReadBool("CountStealthSteps") && World.Player != null)
			{
				m_Count++;
				RazorEnhanced.Misc.SendMessageNoWait(MsgLevel.Error, LocString.StealthSteps, m_Count);
				if (m_Count > 30)
					Unhide();
			}
		}

		internal static void Hide()
		{
			m_Hidden = true;
			m_Count = 0;
			if (RazorEnhanced.Settings.General.ReadBool("CountStealthSteps") && World.Player != null)
				RazorEnhanced.Misc.SendMessageNoWait(MsgLevel.Error, LocString.StealthStart);
		}

		internal static void Unhide()
		{
			m_Hidden = false;
			m_Count = 0;
		}
	}
}