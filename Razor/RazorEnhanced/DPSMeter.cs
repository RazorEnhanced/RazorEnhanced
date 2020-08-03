using Assistant;
using Assistant.UI;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System;

namespace RazorEnhanced
{
	internal class DPSMeter
	{
		public class DamageData
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Damage;
			public int Damage { get { return m_Damage; } set { m_Damage = value; } }

			public DamageData(string name, int damage)
			{
				m_Name = name;
				m_Damage = damage;
			}
		}

		internal static bool Enabled = false;
		private static ConcurrentDictionary<uint, DamageData> m_damagedata = new ConcurrentDictionary<uint, DamageData>();

		internal static void AddDamage(uint serial, ushort damage)
		{
			if (World.Player.Serial == serial)
				return;

			if (m_damagedata.ContainsKey(serial))
			{
				m_damagedata[serial].Damage += damage;
			}
			else
			{
				Assistant.Mobile mob = World.FindMobile(serial);
				if (mob != null)
				{
					DamageData data = new DamageData(World.FindMobile(serial).Name, damage);
					m_damagedata.TryAdd(serial, data);
				}
			}
		}

		internal static void Clear()
		{
			m_damagedata.Clear();
		}

		internal static void ShowResult(DataGridView DpsMeterGridView, int max, int min, int serial, string name)
		{
			DpsMeterGridView.Rows.Clear();

			foreach (KeyValuePair<uint, DPSMeter.DamageData> data in m_damagedata)
			{
				if (serial != -1 && data.Key != serial) // filtro serial attivo
					continue;

				if (max != -1 && data.Value.Damage >= max) // filtro max attivo
					continue;

				if (min != -1 && data.Value.Damage <= min) // filtro min attivo
					continue;

				if (name != null && !data.Value.Name.Contains(name)) // filtro nome attivo
					continue;

				DataGridViewRow row = DpsMeterGridView.Rows[DpsMeterGridView.Rows.Add()];
				// Add data
				row.Cells[0].Value = "0x" + data.Key.ToString("X8");
				row.Cells[1].Value = data.Value.Name;
				row.Cells[2].Value = data.Value.Damage;
			}
		}

		internal static void ShowResult(DataGridView DpsMeterGridView)
		{
			ShowResult(DpsMeterGridView, -1, -1, -1, null);
		}

		// Parte comandi da script

		public static bool Status()
		{
			return Enabled;
		}

		public static void Start()
		{
			if (Assistant.Engine.MainWindow.DPSMeterStartB.Enabled)
				Engine.MainWindow.SafeAction(s => s.DPSMeterStartB.PerformClick());
		}
		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.DPSMeterStopB.Enabled)
				Engine.MainWindow.SafeAction(s => s.DPSMeterStopB.PerformClick());
		}
		public static void Pause()
		{
			if (Assistant.Engine.MainWindow.DPSMeterPauseB.Enabled)
				Engine.MainWindow.SafeAction(s => s.DPSMeterPauseB.PerformClick());
		}

		public static int GetDamage(int serial)
		{
			if (m_damagedata.ContainsKey((uint)serial))
				return m_damagedata[(uint)serial].Damage;

			return 0;
		}
	}
}
