using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace cs2_sonar
{
    public class Base
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        public readonly int VK_F1 = 0x70;

        public string ProcessName { get; private set; } = "cs2";
        public string ModuleName { get; private set; } = "client.dll";
        public int ProcessId { get; private set; }
        public IntPtr ProcessHandle { get; private set; }
        public IntPtr ModuleBase { get; private set; }
        public IntPtr LocalPlayer { get; private set; }

        public bool Initialize()
        {
            ProcessId = Memory.GetProcessIdByName(ProcessName);
            if (ProcessId == 0)
            {
                Console.WriteLine($"{ProcessName} not found.");
                return false;
            }

            ProcessHandle = Memory.OpenProcess(Memory.PROCESS_ALL_ACCESS, false, ProcessId);
            if (ProcessHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to open process handle.");
                return false;
            }

            ModuleBase = Memory.GetModuleBaseAddress(ProcessId, ModuleName);
            if (ModuleBase == IntPtr.Zero)
            {
                Console.WriteLine($"Module '{ModuleName}' not found.");
                return false;
            }

            LocalPlayer = Memory.MemoryRead<IntPtr>(ProcessHandle, ModuleBase + Offsets.LocalPlayerPawn);
            if (LocalPlayer == IntPtr.Zero)
            {
                Console.WriteLine("Failed to find local player.");
                return false;
            }

            return true;
        }
    }
}
