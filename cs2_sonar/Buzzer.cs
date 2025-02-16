using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs2_sonar
{
    public class Buzzer
    {
        public static void BuzzerModule(Base cheatbase, CancellationToken token)
        {

            while (true)
            {
                int entityindex = Memory.MemoryRead<int>(cheatbase.ProcessHandle, cheatbase.LocalPlayer + Offsets.m_iIDEntIndex);

                while (entityindex > 0)
                {
                    Console.Beep(150, 500);
                    break;
                }

                if (token.IsCancellationRequested)
                    break;
            }
        }
    }
}
