using Assistant;
using System;
using System.Media;
using System.Threading;

namespace RazorEnhanced
{
	public class Misc
	{
		//General
		public static void Pause(int mseconds)
		{
			System.Threading.Thread.Sleep(mseconds);
		}

		public static void Resync()
		{
			Assistant.ClientCommunication.SendToServer(new ResyncReq());
		}

		public static double DistanceSqrt(Point3D a, Point3D b)
		{
			double distance = Math.Sqrt(((a.X - b.X) ^ 2) + (a.Y - b.Y) ^ 2);
			return distance;
		}

		// Sysmessage
		public static void SendMessage(int num)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, num.ToString());
		}

		public static void SendMessage(object obj)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, obj.ToString());
		}

		public static void SendMessage(uint num)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, num.ToString());
		}

		public static void SendMessage(string msg)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, msg);
		}

		public static void SendMessage(bool msg)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, msg.ToString());
		}

		public static void SendMessage(double msg)
		{
			if (Assistant.World.Player != null)
				Assistant.World.Player.SendMessage(MsgLevel.Info, msg.ToString());
		}

		public static void SendMessage(int num, int color)
		{
			if (Assistant.World.Player != null)
				ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", num.ToString()));
		}

		public static void SendMessage(object obj, int color)
		{
			if (Assistant.World.Player != null)
				ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", obj.ToString()));
		}

		public static void SendMessage(uint num, int color)
		{
			if (Assistant.World.Player != null)
				ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", num.ToString()));
		}

		public static void SendMessage(string msg, int color)
		{
			if (Assistant.World.Player != null)
				ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
		}

		public static void SendMessage(bool msg, int color)
		{
			if (Assistant.World.Player != null)
				ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
		}

		public static void SendMessage(double msg, int color)
		{
			if (Assistant.World.Player != null)
				ClientCommunication.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));

		}



		public static void Beep()
		{
			SystemSounds.Beep.Play();
		}

		// Login and logout
		public static void Disconnect()
		{
			Assistant.ClientCommunication.SendToClient(new Disconnect());
		}

		// Context Menu

		public static void WaitForContext(Mobile mob, int delay) // Delay in MS
		{
			WaitForContext(mob.Serial, delay);
        }

		public static void WaitForContext(Item i, int delay) // Delay in MS
		{
			WaitForContext(i.Serial, delay);
		}

        public static void WaitForContext(int ser, int delay) // Delay in MS
		{
			ClientCommunication.SendToServer(new ContextMenuRequest(ser));
			int subdelay = delay;
			while (World.Player.HasContext != true && World.Player.ContextID != ser && subdelay > 0)
			{
				Thread.Sleep(2);
				subdelay -= 2;
			}
		}


		public static void ContextReply(int serial, int idx)
		{
			ClientCommunication.SendToServer(new ContextMenuResponse(serial, (ushort)idx));
			World.Player.HasContext = false;
			World.Player.ContextID = 0;
		}

		public static void ContextReply(Mobile mob, int idx)
		{
			ContextReply(mob.Serial, idx);
        }

		public static void ContextReply(Item item, int idx)
		{
			ContextReply(item.Serial, idx);
		}

		// Prompt Message Stuff
		public static void ResetPrompt()
		{
			World.Player.HasPrompt = false;
		}

		public static bool HasPrompt()
		{
			return World.Player.HasPrompt;
		}

		public static void WaitForPrompt(int delay) // Delay in MS
		{
			int subdelay = delay;
			while (!World.Player.HasPrompt && subdelay > 0)
			{
				Thread.Sleep(2);
				subdelay -= 2;
			}
		}

		public static void CancelPrompt()
		{
			ClientCommunication.SendToServer(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 0, Language.CliLocName, ""));
			World.Player.HasPrompt = false;
		}

		public static void ResponsePrompt(string text)
		{
			ClientCommunication.SendToServer(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 1, Language.CliLocName, text));
			World.Player.HasPrompt = false;
		}

		public static void NoOperation()
		{
			return;
		}

		// Shared Script data
		public static object ReadSharedValue(string name)
		{
			object data = 0;
			if (RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.ContainsKey(name))
				RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.TryGetValue(name, out data);
			return data;
		}

		public static void SetSharedValue(string name, object value)
		{
			RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.AddOrUpdate(name, value, (key, oldValue) => value);
		}
		public static void RemoveSharedValue(string name)
		{
			object data = null;
			RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.TryRemove(name, out data);
		}

		public static bool CheckSharedValue(string name)
		{
			if (RazorEnhanced.Scripts.EnhancedScript.SharedScriptData.ContainsKey(name))
				return true;
			else
				return false;
		}

	}
}