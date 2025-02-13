/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs2_sonar
{
    public class UI
    {
        public static void UIModule(Thread sonarmodule, Thread buzzermodule)
        {
            while (true)
            {
                Console.WriteLine($"(F1) Crosshair sniffer status: {Program.ReturnThreadState(buzzermodule)}");
                Console.WriteLine($"(F2) Sonar status: {Program.ReturnThreadState(sonarmodule)}");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
*/
