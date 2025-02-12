using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cs2_sonar
{
    public class Memory
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        public const int PROCESS_ALL_ACCESS = 0x001F0FFF;

        public static int GetProcessIdByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return (processes.Length > 0) ? processes[0].Id : 0;
        }

        // Function to get module base address
        public static IntPtr GetModuleBaseAddress(int processId, string moduleName)
        {
            IntPtr baseAddress = IntPtr.Zero;
            Process process = Process.GetProcessById(processId);
            foreach (ProcessModule module in process.Modules)
            {
                if (module.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    baseAddress = module.BaseAddress;
                    break;
                }
            }
            return baseAddress;
        }

        // Function to read memory from a process
        public static T MemoryRead<T>(IntPtr processHandle, IntPtr address) where T : struct
        {
            int bytesRead;
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            if (ReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead) && bytesRead == size)
            {
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                T result = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
                handle.Free();
                return result;
            }
            return default(T);
        }
    }
}
