using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cs2_sonar
{
    public class ThreadToggle
    {
        public static bool moduleEnabled = false;
        public static bool sonarEnabled = false;
        public static bool buzzerEnabled = false;

        public static void ToggleModule(int vkey, Action<CancellationToken> ActiveModule, ref bool moduleStatus)
        {
            CancellationTokenSource? cts = null;
            Thread? moduleThread = null;
            while (true)
            {
                if (Base.GetAsyncKeyState(vkey) != 0)
                {
                    if (!moduleEnabled)
                    {
                        cts = new CancellationTokenSource();
                        moduleThread = new Thread(() => ActiveModule(cts.Token));
                        moduleThread.Start();
                        moduleEnabled = true;
                        moduleStatus = true;
                    }
                    else
                    {
                        cts?.Cancel();
                        moduleThread?.Join();
                        moduleEnabled = false;
                        moduleStatus = false;
                    }

                    while (Base.GetAsyncKeyState(vkey) != 0)
                    {
                        Thread.Sleep(50);
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}


