using System;
using System.Runtime.InteropServices;

namespace FastColoredTextBoxNS
{
    public static class PlatformType
    {
        const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        [StructLayout(LayoutKind.Sequential)]
        struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        };

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public static Platform GetOperationSystemPlatform()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Platform.X64;  // we'll only support 64 bit on linux
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var sysInfo = new SYSTEM_INFO();
                GetSystemInfo(ref sysInfo);

                switch (sysInfo.wProcessorArchitecture)
                {
                    case PROCESSOR_ARCHITECTURE_IA64:
                    case PROCESSOR_ARCHITECTURE_AMD64:
                        return Platform.X64;

                    case PROCESSOR_ARCHITECTURE_INTEL:
                        return Platform.X86;
                }
            }

            return Platform.Unknown;
        }
    }

    public enum Platform
    {
        X86,
        X64,
        Unknown
    }

}
