using Microsoft.Win32;
namespace Assistant.Core
{
	class CheckDependency
	{
		internal static bool VCRedist2013()
		{
			string regKey = @"SOFTWARE\Classes\Installer\Dependencies\";
			using (Microsoft.Win32.RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey(regKey))
			{
				if (uninstallKey != null)
				{
					string[] productKeys = uninstallKey.GetSubKeyNames();
					foreach (var keyName in productKeys)
					{

						if (keyName == "{050d4fc8-5d48-4b8f-8972-47c82c46020f}" || // Microsoft Visual C++ 2013 Redistributable (x64)
							keyName == "{DA5E371C-6333-3D8A-93A4-6FD5B20BCC6E}" // Microsoft Visual C++ 2013 Redistributable (x86) 

					)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
