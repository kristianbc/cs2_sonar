using System.Numerics;

namespace cs2_sonar
{
    internal class Program
    {
        static void Main()
        {
            string pname = "cs2";
            string mname = "client.dll";

            try
            {
                int processId = Memory.GetProcessIdByName(pname);
                if (processId == 0)
                {
                    Console.WriteLine("cs2 not found");
                    return;
                }

                IntPtr processHandle = Memory.OpenProcess(Memory.PROCESS_ALL_ACCESS, false, processId);

                if (processHandle == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to open process handle.");
                    return;
                }

                IntPtr moduleBase = Memory.GetModuleBaseAddress(processId, mname);
                if (moduleBase == IntPtr.Zero)
                {
                    Console.WriteLine($"Module '{mname}' not found.");
                    return;
                }

                IntPtr localPlayer = Memory.MemoryRead<IntPtr>(processHandle, moduleBase + Offsets.LocalPlayerPawn);
                if (localPlayer == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to find local player.");
                    return;
                }

                IntPtr entitylist = Memory.MemoryRead<IntPtr>(processHandle, moduleBase + Offsets.dwEntityList);
                IntPtr listEntry = Memory.MemoryRead<IntPtr>(processHandle, entitylist + 0x10);

                while (true)
                {
                    List<float> distances = new List<float>();

                    for (int i = 2; i < 64; i++)
                    {
                        if (listEntry == IntPtr.Zero) //skip entire loop if listentry pointer is null
                            continue;

                        IntPtr currentController = Memory.MemoryRead<IntPtr>(processHandle, listEntry + i * 0x78);

                        if (currentController == IntPtr.Zero)
                            continue;

                        int pawnHandle = Memory.MemoryRead<int>(processHandle, currentController + Offsets.m_hPlayerPawn);

                        if (pawnHandle == 0)
                            continue;

                        IntPtr listEntry2 = Memory.MemoryRead<IntPtr>(processHandle, entitylist + 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);

                        IntPtr currentPawn = Memory.MemoryRead<IntPtr>(processHandle, listEntry2 + 0x78 * (pawnHandle & 0x1FF));

                        Vector3 entitypos = Memory.MemoryRead<Vector3>(processHandle, currentPawn + Offsets.m_VecOrigin);
                        Vector3 localpos = Memory.MemoryRead<Vector3>(processHandle, localPlayer + Offsets.m_VecOrigin);

                        float distance = (float)Math.Sqrt(
                        Math.Pow(entitypos.X - localpos.X, 2) +
                        Math.Pow(entitypos.Y - localpos.Y, 2) +
                        Math.Pow(entitypos.Z - localpos.Z, 2)
                        ) * 0.0254f;

                        distances.Add(distance);
                    }

                    float closestDistance = distances.Min();

                    int frequency = (int)(1000 - (closestDistance * 8));
                    frequency = Math.Clamp(frequency, 200, 1000);

                    int duration = 100;
                    int delay = (int)(closestDistance * 10);
                    delay = Math.Clamp(delay, 50, 1000);

                    Console.Beep(frequency, duration);
                    Thread.Sleep(delay);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
