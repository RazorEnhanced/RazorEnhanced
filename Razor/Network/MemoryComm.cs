using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Assistant
{
	public class MemoryComm
	{
		private struct StrInt
		{
			public string Str;
			public uint Val;
		}

		//private readonly ClientVar _currentClientVar;
		private uint _base;
		private const uint BaseAddress = 0x400600;
		private static IntPtr _handleToUo = IntPtr.Zero;
		private readonly int _pathFindVersion;

		public MemoryComm()
		{
			if (_handleToUo == IntPtr.Zero)
				_handleToUo = DLLImport.Win.OpenProcess(2035711, false, ClientCommunication.ClientProcess.Id);

			if (Engine.ClientVersion.Major >= 7)
				_pathFindVersion = 1;
			else if (Engine.ClientVersion.Major == 6)
			{
				if (Engine.ClientVersion.Minor > 0)
					_pathFindVersion = 1;
				else // Minor = 0
				{
					if (Engine.ClientVersion.Build > 6)
						_pathFindVersion = 1;
					else if (Engine.ClientVersion.Build == 6)
					{
						if (Engine.ClientVersion.Revision >= 2)
							_pathFindVersion = 1;
					}
				}
			}
		}

		//private string _data;
		private List<byte> _dataList;
		private List<StrInt> _offsets;
		private List<StrInt> _relBs;
		private Dictionary<string, uint> _labels;

		public void PathFind(short x, short y, short z)
		{
			InitEvents();
			Init(BaseAddress + 0x20);
			AddHex("6A00"); // push dword ptr 0
			AddHex("6A00"); // push dword ptr 0
			AddHex("68"); // push W1
			AddOffs("W1"); //
			AddHex("51"); // push ecx

			if (_pathFindVersion < 1) // ; only required for older clients:
			{
				AddHex("53");               // push ebx
				AddHex("55");               // push ebp
				AddHex("56");               // push esi
				AddHex("57"); // push edi
			}

			AddHex("B8"); // mov eax, Pos
			AddOffs("Pos"); //
			AddHex("83E824"); // sub eax, 24h
			AddHex("E9"); // jmp EPATHFINDADDR
			AddRelA((uint)ClientCommunication.PathFindAddress); //
			AddLabel("W1"); // W1:
			AddHex("C3"); // ret
			AddLabel("Pos"); // Pos:
			AddWVal(x); // dw X
			AddWVal(y); // dw Y
			AddWVal(z); // dw Z
			Final();
			WMem(BaseAddress + 0x20, _dataList.ToArray(), _dataList.Count);
			WMem(BaseAddress - 2, BitConverter.GetBytes((short)1), 2);
			WaitTillProcessed();
		}

		private static void WMem(uint memPos, byte[] data, int length)
		{
			var address = new IntPtr(memPos);
			DLLImport.Win.WriteProcessMemory(_handleToUo, address, data, Convert.ToUInt32(length), out var dummy);
		}

		private static short RMemShort(uint memPos)
		{
			var address = new IntPtr(memPos);
			var byteArray = new byte[2];
			DLLImport.Win.ReadProcessMemory(_handleToUo, address, byteArray, 2, out var dummy);
			return BitConverter.ToInt16(byteArray, 0);
		}

		private static void WaitTillProcessed()
		{
			for (var i = 1; i < 20; i++)
			{
				Thread.Sleep(25);
				var w = RMemShort(BaseAddress - 2);
				if (w > 0)
					continue;
				break;
			}

			WMem(BaseAddress - 2, BitConverter.GetBytes((short)0), 2); // The process failed, but to avoid a lock, we set again to 0
		}

		private void InitEvents()
		{
			DLLImport.Win.VirtualProtectEx(_handleToUo, new IntPtr(0x400000), new UIntPtr(0x1000), 0x00000040, out var dummy);

			Init(BaseAddress - 4);
			AddWVal(0x0000);           // dw 0000
			AddLabel("Flag2");        // Flag2:
			AddWVal(0x0000);           // dw 0000
			AddHex("68");             // push EOLDDIR
			AddCVal((uint)ClientCommunication.E_OLDDIRAddress);   //
			AddHex("66833D");         // cmp word ptr [Flag2], 0000
			AddOffs("Flag2");       //
			AddHex("00");           //
			AddHex("75");             // jne W1
			AddRelB("W1");          //
			AddHex("C3");             // ret
			AddLabel("W1");           // W1:
			AddHex("66C705");         // mov word ptr [Flag2], 0000
			AddOffs("Flag2");       //
			AddHex("0000");         //
			AddHex("90909090909090"); // nop (7x)
			Final();
			WMem(BaseAddress - 4, _dataList.ToArray(), _dataList.Count);

			Init((uint)ClientCommunication.E_REDIRAddress);
			AddHex("E8");             // call BaseAddr
			AddRelA(BaseAddress);      //
			Final();
			WMem((uint)ClientCommunication.E_REDIRAddress, _dataList.ToArray(), _dataList.Count);
		}

		private void Init(uint c)
		{
			_dataList = new List<byte>();
			_offsets = new List<StrInt>();
			_relBs = new List<StrInt>();
			_labels = new Dictionary<string, uint>();
			_base = c;
		}

		private void AddHex(string s)
		{
			var bytes = StringToByteArray(s);
			foreach (var byt in bytes)
			{
				_dataList.Add(byt);
			}
		}

		private static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();
		}

		private void AddOffs(string s)
		{
			var newStrInt = new StrInt
			{
				Str = s,
				Val = Convert.ToUInt32(_dataList.Count)
			};
			_offsets.Add(newStrInt);
			_dataList.Add(0);
			_dataList.Add(0);
			_dataList.Add(0);
			_dataList.Add(0);
		}

		private void AddLabel(string s)
		{
			_labels.Add(s, Convert.ToUInt32(_dataList.Count));
		}

		private void AddRelA(uint c)
		{
			_dataList.Add(0);
			_dataList.Add(0);
			_dataList.Add(0);
			_dataList.Add(0);
			var d = Convert.ToUInt32(_dataList.Count);
			c = c - (_base + d);

			var bytesC = BitConverter.GetBytes(c);
			for (var i = 0; i < 4; i++)
				_dataList[Convert.ToInt32(d - 4 + i)] = bytesC[i];
		}

		private void AddRelB(string s)
		{
			var newStrInt = new StrInt
			{
				Str = s,
				Val = Convert.ToUInt32(_dataList.Count)
			};
			_relBs.Add(newStrInt);
			_dataList.Add(0);
		}

		private void AddWVal(short w)
		{
			foreach (var byt in BitConverter.GetBytes(w))
				_dataList.Add(byt);
		}

		private void AddCVal(uint c)
		{
			foreach (var byt in BitConverter.GetBytes(c))
				_dataList.Add(byt);
		}

		private void Final()
		{
			foreach (var offsetKey in _offsets)
			{
				if (!FindLabel(offsetKey.Str, out var c))
					return;

				var trg = offsetKey.Val;
				var src = _base + c;

				var bytesSrc = BitConverter.GetBytes(src);
				for (var i = 0; i < 4; i++)
					_dataList[Convert.ToInt32(trg + i)] = bytesSrc[i];
			}

			foreach (var relBKey in _relBs)
			{
				if (!FindLabel(relBKey.Str, out var c))
					return;

				var trg = relBKey.Val;
				var src = Convert.ToByte(c - (trg + 1));

				var bytesSrc = BitConverter.GetBytes(src);
				for (var i = 0; i < 1; i++)
					_dataList[Convert.ToInt32(trg + i)] = bytesSrc[i];

			}
		}

		private bool FindLabel(string s, out uint c)
		{
			c = 0;

			if (_labels.ContainsKey(s))
			{
				c = _labels[s];
				return true;
			}

			return false;
		}
	}
}
