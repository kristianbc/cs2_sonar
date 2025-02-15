using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace cs2_sonar
{
    public class Sonar
    {
        public static void SonarModule(Base cheatbase)
        {
            try
            {
                IntPtr entitylist = Memory.MemoryRead<IntPtr>(cheatbase.ProcessHandle, cheatbase.ModuleBase + Offsets.dwEntityList);
                IntPtr listEntry = Memory.MemoryRead<IntPtr>(cheatbase.ProcessHandle, entitylist + 0x10);

                while (true)
                {
                    IntPtr engineNetworkPointer = Memory.MemoryRead<IntPtr>(cheatbase.ProcessHandle, cheatbase.ModuleBase + Offsets.dwNetworkGameClient);
                    bool isInMenu = Memory.MemoryRead<bool>(cheatbase.ProcessHandle, engineNetworkPointer + Offsets.dwNetworkGameClient_isBackgroundMap);

                    if (isInMenu == false)
                    {
                        List<float> distances = new List<float>();

                        for (int i = 2; i < 64; i++)
                        {
                            if (listEntry == IntPtr.Zero) //skip entire loop if listentry pointer is null
                                continue;

                            IntPtr currentController = Memory.MemoryRead<IntPtr>(cheatbase.ProcessHandle, listEntry + i * 0x78);

                            if (currentController == IntPtr.Zero)
                                continue;

                            int pawnHandle = Memory.MemoryRead<int>(cheatbase.ProcessHandle, currentController + Offsets.m_hPlayerPawn);

                            if (pawnHandle == 0)
                                continue;

                            IntPtr listEntry2 = Memory.MemoryRead<IntPtr>(cheatbase.ProcessHandle, entitylist + 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                            IntPtr currentPawn = Memory.MemoryRead<IntPtr>(cheatbase.ProcessHandle, listEntry2 + 0x78 * (pawnHandle & 0x1FF));

                            Vector3 entitypos = Memory.MemoryRead<Vector3>(cheatbase.ProcessHandle, currentPawn + Offsets.m_VOldOrigin);
                            Vector3 localpos = Memory.MemoryRead<Vector3>(cheatbase.ProcessHandle, cheatbase.LocalPlayer + Offsets.m_VOldOrigin);

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
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Make sure you're ingame. ERROR: {ex.Message}");
            }
            
        }
    }
}
